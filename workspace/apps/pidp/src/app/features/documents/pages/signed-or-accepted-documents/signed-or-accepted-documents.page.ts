import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { combineLatest, map } from 'rxjs';

import { PartyService } from '@app/core/party/party.service';
import {
  DocumentService,
  DocumentType,
  IDocumentMetaData,
} from '@app/core/services/document.service';
import { IdentityProvider } from '@app/features/auth/enums/identity-provider.enum';
import { AuthorizedUserService } from '@app/features/auth/services/authorized-user.service';
import { StatusCode } from '@app/features/portal/enums/status-code.enum';
import { ProfileStatus } from '@app/features/portal/models/profile-status.model';

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

  public constructor(
    private route: ActivatedRoute,
    private router: Router,
    private partyService: PartyService,
    private resource: SignedOrAcceptedDocumentsResource,
    private authUserService: AuthorizedUserService,
    private documentService: DocumentService
  ) {
    this.title = route.snapshot.data.title;
    this.documents = [];
  }

  public onViewDocument(documentType: DocumentType): void {
    this.router.navigate([
      DocumentsRoutes.MODULE_PATH,
      DocumentsRoutes.VIEW_DOCUMENT_PAGE,
      documentType,
    ]);
  }

  public onBack(): void {
    this.navigateToRoot();
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

  private navigateToRoot(): void {
    this.router.navigate([this.route.snapshot.data.routes.root]);
  }
}
