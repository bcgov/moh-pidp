import { NgIf } from '@angular/common';
import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';
import { AfterViewInit, Component, OnInit } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';

import { catchError, noop, of, tap } from 'rxjs';

import {
  LOADING_OVERLAY_DEFAULT_MESSAGE,
  LoadingOverlayService,
} from '@pidp/presentation';

import {
  AnchorDirective,
  InjectViewportCssClassDirective,
  PageFooterActionDirective,
} from '@bcgov/shared/ui';

import { PartyService } from '@app/core/party/party.service';
import { LoggerService } from '@app/core/services/logger.service';
import { SnowplowService } from '@app/core/services/snowplow.service';
import { StatusCode } from '@app/features/portal/enums/status-code.enum';
import { BreadcrumbComponent } from '@app/shared/components/breadcrumb/breadcrumb.component';

import { AccessRoutes } from '../../access.routes';
import { EnrolmentErrorComponent } from '../../components/enrolment-error/enrolment-error.component';
import { DriverFitnessResource } from './driver-fitness-resource.service';
import {
  driverFitnessSupportEmail,
  medicalPractitionerPortalUrl,
} from './driver-fitness.constants';

@Component({
  selector: 'app-driver-fitness',
  templateUrl: './driver-fitness.page.html',
  styleUrls: ['./driver-fitness.page.scss'],
  standalone: true,
  imports: [
    AnchorDirective,
    BreadcrumbComponent,
    EnrolmentErrorComponent,
    InjectViewportCssClassDirective,
    MatButtonModule,
    NgIf,
    PageFooterActionDirective,
    RouterLink,
  ],
})
export class DriverFitnessPage implements OnInit, AfterViewInit {
  public completed: boolean | null;
  public accessRequestFailed: boolean;
  public driverFitnessSupportEmail: string;
  public enrolmentError: boolean;
  public medicalPractitionerPortalUrl: string;
  public AccessRoutes = AccessRoutes;
  public breadcrumbsData: Array<{ title: string; path: string }> = [
    { title: 'Home', path: '' },
    {
      title: 'Access',
      path: AccessRoutes.routePath(AccessRoutes.ACCESS_REQUESTS),
    },
    { title: 'DMFT', path: '' },
  ];

  public constructor(
    private readonly loadingOverlayService: LoadingOverlayService,
    private readonly route: ActivatedRoute,
    private readonly router: Router,
    private readonly partyService: PartyService,
    private readonly resource: DriverFitnessResource,
    private readonly logger: LoggerService,
    private readonly snowplowService: SnowplowService,
  ) {
    const routeData = this.route.snapshot.data;
    this.completed = routeData.driverFitnessStatusCode === StatusCode.COMPLETED;
    this.accessRequestFailed = false;
    this.driverFitnessSupportEmail = driverFitnessSupportEmail;
    this.enrolmentError = false;
    this.medicalPractitionerPortalUrl = medicalPractitionerPortalUrl;
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

  public onRequestAccess(): void {
    this.loadingOverlayService.open(LOADING_OVERLAY_DEFAULT_MESSAGE);
    this.resource
      .requestAccess(this.partyService.partyId)
      .pipe(
        tap(() => {
          this.completed = true;
          this.loadingOverlayService.close();
          this.enrolmentError = false;
        }),
        catchError((error: HttpErrorResponse) => {
          this.loadingOverlayService.close();
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

  private navigateToRoot(): void {
    this.router.navigate(['/']);
  }
}
