import { NgIf } from '@angular/common';
import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';
import { AfterViewInit, Component, OnInit } from '@angular/core';
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
import { SnowplowService } from '@app/core/services/snowplow.service';
import { StatusCode } from '@app/features/portal/enums/status-code.enum';
import { BreadcrumbComponent } from '@app/shared/components/breadcrumb/breadcrumb.component';

import { AccessRoutes } from '../../access.routes';
import { EnrolmentErrorComponent } from '../../components/enrolment-error/enrolment-error.component';
import { PrescriptionRefillEformsResource } from './prescription-refill-eforms-resource.service';
import {
  prescriptionRefillRequestEformsSupportEmail,
  prescriptionRefillRequestEformsUrl,
} from './prescription-refill-eforms.constants';

@Component({
    selector: 'app-prescription-refill-eforms',
    templateUrl: './prescription-refill-eforms.page.html',
    styleUrls: ['./prescription-refill-eforms.page.scss'],
    imports: [
        AlertComponent,
        AlertContentDirective,
        AnchorDirective,
        BreadcrumbComponent,
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
    ]
})
export class PrescriptionRefillEformsPage implements OnInit, AfterViewInit {
  public title: string;
  public collectionNotice: string;
  public completed: boolean | null;
  public accessRequestFailed: boolean;
  public prescriptionRefillEformsUrl: string;
  public prescriptionRefillEformsSupportEmail: string;
  public enrolmentError: boolean;
  public AccessRoutes = AccessRoutes;
  public breadcrumbsData: Array<{ title: string; path: string }> = [
    { title: 'Home', path: '' },
    {
      title: 'Access',
      path: AccessRoutes.routePath(AccessRoutes.ACCESS_REQUESTS),
    },
    { title: 'Prescription Refill Eforms', path: '' },
  ];

  public constructor(
    private route: ActivatedRoute,
    private router: Router,
    private partyService: PartyService,
    private resource: PrescriptionRefillEformsResource,
    private logger: LoggerService,
    private documentService: DocumentService,
    private snowplowService: SnowplowService,
  ) {
    const routeData = this.route.snapshot.data;
    this.title = routeData.title;
    this.collectionNotice =
      documentService.getPrescriptionRefilleFormsCollectionNotice();
    this.completed =
      routeData.prescriptionRefillEformsStatusCode === StatusCode.COMPLETED;
    this.accessRequestFailed = false;
    this.enrolmentError = false;
    this.prescriptionRefillEformsUrl = prescriptionRefillRequestEformsUrl;
    this.prescriptionRefillEformsSupportEmail =
      prescriptionRefillRequestEformsSupportEmail;
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

  public ngAfterViewInit(): void {
    this.snowplowService.refreshLinkClickTracking();
  }

  private navigateToRoot(): void {
    this.router.navigate([this.route.snapshot.data.routes.root]);
  }
}
