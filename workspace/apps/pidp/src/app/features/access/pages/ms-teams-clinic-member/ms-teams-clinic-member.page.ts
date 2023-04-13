import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { MatSelectChange } from '@angular/material/select';
import { ActivatedRoute, Router } from '@angular/router';

import { EMPTY, catchError, noop, of, tap } from 'rxjs';

import {
  LOADING_OVERLAY_DEFAULT_MESSAGE,
  LoadingOverlayService,
} from '@pidp/presentation';

import { NoContent } from '@bcgov/shared/data-access';

import {
  AbstractFormDependenciesService,
  AbstractFormPage,
} from '@app/core/classes/abstract-form-page.class';
import { PartyService } from '@app/core/party/party.service';
import { LoggerService } from '@app/core/services/logger.service';
import { StatusCode } from '@app/features/portal/enums/status-code.enum';

import { msTeamsSupportEmail } from '../ms-teams-privacy-officer/ms-teams.constants';
import { MsTeamsClinicMemberFormState } from './ms-teams-clinic-member-form-state';
import { MsTeamsClinicMemberResource } from './ms-teams-clinic-member-resource.service';
import { PrivacyOfficer } from './ms-teams-clinic-member.model';

@Component({
  selector: 'app-ms-teams-clinic-member',
  templateUrl: './ms-teams-clinic-member.page.html',
  styleUrls: ['./ms-teams-clinic-member.page.scss'],
})
export class MsTeamsClinicMemberPage
  extends AbstractFormPage<MsTeamsClinicMemberFormState>
  implements OnInit
{
  public completed: boolean | null;
  public msTeamsSupportEmail: string;
  public selectedPrivacyOfficer: PrivacyOfficer | null;
  public clinicId!: number;

  public accessRequestFailed = false;
  public enrolmentError = false;
  public formState: MsTeamsClinicMemberFormState;
  public showOverlayOnSubmit = false;
  public privacyOfficers!: PrivacyOfficer[] | null;

  public constructor(
    dependenciesService: AbstractFormDependenciesService,
    private route: ActivatedRoute,
    private router: Router,
    private partyService: PartyService,
    private resource: MsTeamsClinicMemberResource,
    private logger: LoggerService,
    fb: FormBuilder,
    private loadingOverlayService: LoadingOverlayService
  ) {
    super(dependenciesService);
    const routeData = this.route.snapshot.data;
    this.completed =
      routeData.msTeamsClinicMemberStatusCode === StatusCode.COMPLETED;
    this.msTeamsSupportEmail = msTeamsSupportEmail;

    this.formState = new MsTeamsClinicMemberFormState(
      fb,
      dependenciesService.formUtilsService
    );
    this.selectedPrivacyOfficer = null;
  }

  public onSelectPrivacyOfficer(selection: MatSelectChange): void {
    this.selectedPrivacyOfficer = selection.value;
    this.clinicId = selection.value.clinicId;
  }

  public onBack(): void {
    this.navigateToRoot();
  }

  public onRequestAccess(): void {
    this.loadingOverlayService.open(LOADING_OVERLAY_DEFAULT_MESSAGE);
    this.resource
      .requestAccess(this.partyService.partyId, this.clinicId)
      .pipe(
        tap(() => {
          this.loadingOverlayService.close();
          this.completed = true;
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

    this.resource
      .getPrivacyOfficers(partyId)
      .subscribe((privacyOfficers) => (this.privacyOfficers = privacyOfficers));
  }

  protected performSubmission(): NoContent {
    const partyId = this.partyService.partyId;

    this.loadingOverlayService.open();

    return partyId && this.formState.json
      ? this.resource
          .requestAccess(this.partyService.partyId, this.clinicId)
          .pipe(
            tap(() => {
              this.loadingOverlayService.close();
              this.completed = true;
              this.enrolmentError = false;
            }),
            catchError((error: HttpErrorResponse) => {
              this.loadingOverlayService.close();
              if (error.status === HttpStatusCode.BadRequest) {
                this.completed = false;
                this.enrolmentError = true;
                return of(noop());
              }
              return of(noop());
            })
          )
      : EMPTY;
  }

  private navigateToRoot(): void {
    this.router.navigate([this.route.snapshot.data.routes.root]);
  }
}
