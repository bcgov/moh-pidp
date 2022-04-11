import { Location } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { combineLatest, map } from 'rxjs';

import { RouteUtils } from '@bcgov/shared/utils';

import { PartyService } from '@app/core/party/party.service';
import {
  DocumentService,
  DocumentType,
  IDocumentMetaData,
} from '@app/core/services/document.service';
import { IdentityProvider } from '@app/features/auth/enums/identity-provider.enum';
import { AuthorizedUserService } from '@app/features/auth/services/authorized-user.service';
import { StatusCode } from '@app/features/portal/enums/status-code.enum';
import { ProfileStatus } from '@app/features/portal/sections/models/profile-status.model';

import { DocumentsRoutes } from '../../documents.routes';
import { SignedOrAcceptedDocumentsResource } from './signed-or-accepted-documents-resource.service';

export interface DocumentSection extends IDocumentMetaData {
  actionLabel?: string;
  disabled?: boolean;
}

@Component({
  selector: 'app-signed-or-accepted-documents',
  templateUrl: './signed-or-accepted-documents.page.html',
  styleUrls: ['./signed-or-accepted-documents.page.scss'],
})
export class SignedOrAcceptedDocumentsPage implements OnInit {
  public title: string;
  public documents: DocumentSection[];
  private routeUtils: RouteUtils;

  public constructor(
    private partyService: PartyService,
    private resource: SignedOrAcceptedDocumentsResource,
    private authUserService: AuthorizedUserService,
    private documentService: DocumentService,
    route: ActivatedRoute,
    router: Router,
    location: Location
  ) {
    this.title = route.snapshot.data.title;
    this.documents = [];
    // TODO move into provider for each module and DI into
    // components to reduce redundant initialization
    this.routeUtils = new RouteUtils(
      route,
      router,
      DocumentsRoutes.MODULE_PATH,
      location
    );
  }

  public onViewDocument(documentType: DocumentType): void {
    this.routeUtils.routeWithin([
      DocumentsRoutes.VIEW_DOCUMENT_PAGE,
      documentType,
    ]);
  }

  public ngOnInit(): void {
    const partyId = this.partyService.partyId;

    // TODO filtering has been isolated to a single place in the
    // application to reduce complexity until a system around
    // available documents is built
    combineLatest([
      this.authUserService.identityProvider$,
      this.resource.getProfileStatus(partyId),
    ])
      .pipe(
        map(
          (result: [IdentityProvider, ProfileStatus | null]) =>
            (this.documents = this.getDocuments(...result))
        )
      )
      .subscribe();
  }

  private getDocuments(
    identityProvider: IdentityProvider,
    profileStatus: ProfileStatus | null
  ): DocumentSection[] {
    const documents = this.documentService.getDocuments();

    return documents
      .filter((document: IDocumentMetaData) => {
        if (
          document.type === DocumentType.SA_EFORMS_COLLECTION_NOTICE &&
          profileStatus?.status.saEforms.statusCode !== StatusCode.COMPLETED
        ) {
          return false;
        }

        return true;
      })
      .map((document) => ({
        ...document,
        actionLabel: 'View',
      }));
  }
}
