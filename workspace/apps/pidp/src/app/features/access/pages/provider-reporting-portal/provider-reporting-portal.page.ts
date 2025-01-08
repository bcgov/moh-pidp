import { NgIf } from '@angular/common';
import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { ActivatedRoute, Router } from '@angular/router';

import { catchError, noop, of, tap } from 'rxjs';

import {
  AlertComponent,
  AlertContentDirective,
  AnchorDirective,
  InjectViewportCssClassDirective,
  PageComponent,
  PageFooterActionDirective,
  PageFooterComponent,
  PageHeaderComponent,
  PageSectionComponent,
  PageSectionSubheaderComponent,
  PageSectionSubheaderDescDirective,
  SafePipe,
} from '@bcgov/shared/ui';

import { PartyService } from '@app/core/party/party.service';
import { DocumentService } from '@app/core/services/document.service';
import { LoggerService } from '@app/core/services/logger.service';
import { StatusCode } from '@app/features/portal/enums/status-code.enum';

import { EnrolmentErrorComponent } from '../../components/enrolment-error/enrolment-error.component';
import { ProviderReportingPortalResource } from './provider-reporting-portal-resource.service';

@Component({
  selector: 'app-provider-reporting-portal',
  templateUrl: './provider-reporting-portal.page.html',
  styleUrls: ['./provider-reporting-portal.page.scss'],
  standalone: true,
  imports: [
    AlertComponent,
    AlertContentDirective,
    AnchorDirective,
    EnrolmentErrorComponent,
    InjectViewportCssClassDirective,
    MatButtonModule,
    NgIf,
    PageComponent,
    PageFooterActionDirective,
    PageFooterComponent,
    PageHeaderComponent,
    PageSectionComponent,
    PageSectionSubheaderComponent,
    PageSectionSubheaderDescDirective,
    SafePipe,
  ],
})
export class ProviderReportingPortalPage implements OnInit {
  public title: string;
  public collectionNotice: string;
  public completed: boolean | null;
  public accessRequestFailed: boolean;
  public enrolmentError: boolean;

  public constructor(
    private route: ActivatedRoute,
    private router: Router,
    private partyService: PartyService,
    private resource: ProviderReportingPortalResource,
    private logger: LoggerService,
    private documentService: DocumentService,
  ) {
    const routeData = this.route.snapshot.data;
    this.title = routeData.title;
    this.collectionNotice =
      documentService.getProviderReportingPortalCollectionNotice();
    this.completed =
      routeData.providerReportingPortalStatusCode === StatusCode.COMPLETED;
    this.accessRequestFailed = false;
    this.enrolmentError = false;
  }

  public onRequestAccess(): void {
    this.resource
      .requestAccess(this.partyService.partyId)
      .pipe(
        tap(() => {
          this.completed = true;
          this.enrolmentError = false;
        }),
        catchError((error: HttpErrorResponse) => {
          if (error.status === HttpStatusCode.BadRequest) {
            this.completed = false;
            this.enrolmentError = true;
            return of(noop());
          }
          this.accessRequestFailed = true;
          return of(noop());
        }),
      )
      .subscribe();
  }

  public ngOnInit(): void {
    const partyId = this.partyService.partyId;

    if (!partyId) {
      this.logger.error('No party ID was provided');
      return this.navigateToRoot();
    }

    if (this.completed === null) {
      this.logger.error('No status code was provided');
      return this.navigateToRoot();
    }
  }

  private navigateToRoot(): void {
    this.router.navigate([this.route.snapshot.data.routes.root]);
  }
}
