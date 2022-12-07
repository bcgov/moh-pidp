import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';
import {
  ChangeDetectionStrategy,
  Component,
  Inject,
  OnInit,
} from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { catchError, noop, of, tap } from 'rxjs';

import { ApplicationService } from '@pidp/presentation';

import { APP_CONFIG, AppConfig } from '@app/app.config';
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
  public title: string;
  public driverFitnessUrl: string;
  public completed: boolean | null;
  public accessRequestFailed: boolean;
  public driverFitnessSupportEmail: string;
  public enrolmentError: boolean;
  public medicalPractitionerPortalUrl: string;

  public constructor(
    @Inject(APP_CONFIG) private config: AppConfig,
    private route: ActivatedRoute,
    private router: Router,
    private partyService: PartyService,
    private resource: DriverFitnessResource,
    private logger: LoggerService,
    private applicationService: ApplicationService
  ) {
    const routeData = this.route.snapshot.data;
    this.title = routeData.title;
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

    // TODO: Remove this temporary code
    const state = this.route.snapshot.queryParamMap.get('state');
    if (state) {
      if (state === '1') {
        this.completed = false;
      } else if (state === '2') {
        this.completed = true;
        this.onAccessGranted();
      } else if (state === '3') {
        this.completed = false;
        this.accessRequestFailed = true;
      }
    }
  }

  public onBack(): void {
    this.navigateToRoot();
  }

  public onRequestAccess(): void {
    this.resource
      .requestAccess(this.partyService.partyId)
      .pipe(
        tap(() => {
          this.completed = true;
        }),
        catchError((error: HttpErrorResponse) => {
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
    this.router.navigate([this.route.snapshot.data.routes.root]);
  }
}
