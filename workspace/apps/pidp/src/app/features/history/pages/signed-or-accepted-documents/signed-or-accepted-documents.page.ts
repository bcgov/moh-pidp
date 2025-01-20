import { NgFor } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { ActivatedRoute, Router } from '@angular/router';

import { map } from 'rxjs';

import {
  CardSummaryComponent,
  PageComponent,
  PageFooterActionDirective,
  PageFooterComponent,
  PageHeaderComponent,
  PageSectionComponent,
  PageSectionSubheaderComponent,
  PageSectionSubheaderDescDirective,
} from '@bcgov/shared/ui';

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
  standalone: true,
  imports: [
    CardSummaryComponent,
    MatButtonModule,
    NgFor,
    PageComponent,
    PageFooterActionDirective,
    PageFooterComponent,
    PageHeaderComponent,
    PageSectionComponent,
    PageSectionSubheaderComponent,
    PageSectionSubheaderDescDirective,
  ],
})
export class SignedOrAcceptedDocumentsPage implements OnInit {
  public title: string;
  public documents: DocumentSection[];

  public constructor(
    private readonly route: ActivatedRoute,
    private readonly router: Router,
    private readonly partyService: PartyService,
    private readonly resource: SignedOrAcceptedDocumentsResource,
    private readonly documentService: DocumentService,
    private readonly permissionsService: PermissionsService,
  ) {
    this.title = route.snapshot.data.title;
    this.documents = [];
  }

  public onViewDocument(documentType: DocumentType): void {
    this.router.navigate([
      HistoryRoutes.BASE_PATH,
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
            (this.documents = this.getDocuments(profileStatus)),
        ),
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

        // TO DO remove when an API or equivalent is available, but until
        // then has to be displayed all the time or none of the time
        status.userAccessAgreement = this.permissionsService.hasRole([
          Role.FEATURE_PIDP_DEMO,
        ])
          ? { statusCode: StatusCode.COMPLETED }
          : { statusCode: StatusCode.NOT_AVAILABLE };

        switch (document.type) {
          case DocumentType.PIDP_COLLECTION_NOTICE:
            return true;

          case DocumentType.USER_ACCESS_AGREEMENT:
            return (
              status?.userAccessAgreement.statusCode === StatusCode.COMPLETED
            );

          case DocumentType.PRESCRIPTION_REFILL_EFORMS_COLLECTION_NOTICE:
            return (
              status?.prescriptionRefillEforms.statusCode ===
              StatusCode.COMPLETED
            );

          case DocumentType.PROVIDER_REPORTING_PORTAL_COLLECTION_NOTICE:
            return (
              status?.providerReportingPortal.statusCode ===
              StatusCode.COMPLETED
            );

          case DocumentType.SA_EFORMS_COLLECTION_NOTICE:
            return status?.saEforms.statusCode === StatusCode.COMPLETED;

          case DocumentType.MS_TEAMS_DECLARATION_AGREEMENT:
          case DocumentType.MS_TEAMS_DETAILS_AGREEMENT:
          case DocumentType.MS_TEAMS_IT_SECURITY_AGREEMENT:
            return (
              status?.msTeamsPrivacyOfficer.statusCode === StatusCode.COMPLETED
            );

          default:
            return false;
        }
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
