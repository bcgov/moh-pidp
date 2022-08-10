import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { map } from 'rxjs';

import { PartyService } from '@app/core/party/party.service';
import {
  DocumentService,
  DocumentType,
  IDocumentMetaData,
} from '@app/core/services/document.service';
import { StatusCode } from '@app/features/portal/enums/status-code.enum';
import { ProfileStatus } from '@app/features/portal/models/profile-status.model';
import { PermissionsService } from '@app/modules/permissions/permissions.service';
import { Role } from '@app/shared/enums/roles.enum';

import { HistoryRoutes } from '../../history.routes';
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
    private documentService: DocumentService,
    private permissionsService: PermissionsService
  ) {
    this.title = route.snapshot.data.title;
    this.documents = [];
  }

  public onViewDocument(documentType: DocumentType): void {
    this.router.navigate([
      HistoryRoutes.MODULE_PATH,
      HistoryRoutes.VIEW_DOCUMENT,
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
    this.resource
      .getProfileStatus(partyId)
      .pipe(
        map(
          (profileStatus: ProfileStatus | null) =>
            (this.documents = this.getDocuments(profileStatus))
        )
      )
      .subscribe();
  }

  private getDocuments(profileStatus: ProfileStatus | null): DocumentSection[] {
    const documents = this.documentService.getDocuments();

    return documents
      .filter((document: IDocumentMetaData) => {
        const status = profileStatus?.status;

        if (!status) {
          return;
        }

        // TODO remove when an API or equivalent is available, but until
        // then has to be displayed all the time or none of the time
        status.userAccessAgreement = this.permissionsService.hasRole([
          Role.FEATURE_PIDP_DEMO,
        ])
          ? { statusCode: StatusCode.COMPLETED }
          : { statusCode: StatusCode.NOT_AVAILABLE };

        return (
          document.type === DocumentType.PIDP_COLLECTION_NOTICE ||
          (document.type === DocumentType.SA_EFORMS_COLLECTION_NOTICE &&
            status?.saEforms.statusCode === StatusCode.COMPLETED) ||
          (document.type === DocumentType.DRIVER_FITNESS_COLLECTION_NOTICE &&
            status?.driverFitness.statusCode === StatusCode.COMPLETED) ||
          (document.type === DocumentType.USER_ACCESS_AGREEMENT &&
            status?.userAccessAgreement.statusCode === StatusCode.COMPLETED) ||
          (document.type === DocumentType.UCI_COLLECTION_NOTICE &&
            status?.uci.statusCode === StatusCode.COMPLETED)
        );
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
