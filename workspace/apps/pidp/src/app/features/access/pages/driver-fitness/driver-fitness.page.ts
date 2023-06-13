import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { catchError, noop, of, tap } from 'rxjs';

import {
  ApplicationService,
  LOADING_OVERLAY_DEFAULT_MESSAGE,
  LoadingOverlayService,
} from '@pidp/presentation';

import { PartyService } from '@app/core/party/party.service';
import { LoggerService } from '@app/core/services/logger.service';
import { StatusCode } from '@app/features/portal/enums/status-code.enum';

import { DriverFitnessResource } from './driver-fitness-resource.service';
import {
  driverFitnessSupportEmail,
  driverFitnessUrl,
  medicalPractitionerPortalUrl,
} from './driver-fitness.constants';

@Component({
  selector: 'app-driver-fitness',
  templateUrl: './driver-fitness.page.html',
  styleUrls: ['./driver-fitness.page.scss'],
})
export class DriverFitnessPage implements OnInit {
  public driverFitnessUrl: string;
  public completed: boolean | null;
  public accessRequestFailed: boolean;
  public driverFitnessSupportEmail: string;
  public enrolmentError: boolean;
  public medicalPractitionerPortalUrl: string;

  public constructor(
    private loadingOverlayService: LoadingOverlayService,
    private route: ActivatedRoute,
    private router: Router,
    private partyService: PartyService,
    private resource: DriverFitnessResource,
    private logger: LoggerService,
    private applicationService: ApplicationService
  ) {
    const routeData = this.route.snapshot.data;
    this.driverFitnessUrl = driverFitnessUrl;
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

  public onBack(): void {
    this.navigateToRoot();
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
        })
      )
      .subscribe((_) => {
        if (this.completed) {
          this.onAccessGranted();
        }
      });
  }
  private onAccessGranted(): void {
    this.applicationService.setDashboardTitleText(
      'Enrolment Completed',
      'Your information has been submitted successfully'
    );
  }

  private navigateToRoot(): void {
    this.router.navigate(['/']);
  }
}
