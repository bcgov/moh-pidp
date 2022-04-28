import { Injectable } from '@angular/core';
import { Router } from '@angular/router';

import { BehaviorSubject, Observable } from 'rxjs';

import { PermissionsService } from '@app/modules/permissions/permissions.service';

import { AlertCode } from './enums/alert-code.enum';
import { StatusCode } from './enums/status-code.enum';
import { ProfileStatusAlert } from './models/profile-status-alert.model';
import { ProfileStatus } from './models/profile-status.model';
import { PortalSectionStatusKey } from './state/portal-section-status-key.type';
import { PortalState, PortalStateBuilder } from './state/portal-state.builder';

@Injectable({
  providedIn: 'root',
})
export class PortalService {
  /**
   * @description
   * Copy of the provided profile status from the most recent
   * update of the service.
   */
  private _profileStatus: ProfileStatus | null;
  private _alerts: ProfileStatusAlert[];
  private _completedProfile: boolean;
  private _state$: BehaviorSubject<PortalState>;

  public constructor(
    private router: Router,
    private permissionsService: PermissionsService
  ) {
    this._profileStatus = null;
    this._state$ = new BehaviorSubject<PortalState>(null);
    this._completedProfile = false;
    this._alerts = [];
  }

  public get state$(): Observable<PortalState> {
    return this._state$.asObservable();
  }

  public get profileStatus(): ProfileStatus | null {
    return this._profileStatus;
  }

  public get completedProfile(): boolean {
    return this._completedProfile;
  }

  public get alerts(): ProfileStatusAlert[] {
    return this._alerts;
  }

  public get collegeLicenceValidationError(): boolean {
    return (
      this._profileStatus?.status.collegeCertification?.statusCode ===
      StatusCode.ERROR
    );
  }

  public get hiddenSections(): PortalSectionStatusKey[] {
    const status = this._profileStatus?.status;

    if (!status) {
      return [];
    }

    return (Object.keys(status) as PortalSectionStatusKey[]).filter(
      (key: PortalSectionStatusKey) =>
        status[key].statusCode === StatusCode.HIDDEN
    );
  }

  public updateState(profileStatus: ProfileStatus | null): void {
    if (!profileStatus) {
      this.clearState();
      return;
    }

    this._profileStatus = profileStatus;
    this._alerts = this.getAlerts(profileStatus);
    this._completedProfile = this.hasCompletedProfile(profileStatus);

    const builder = new PortalStateBuilder(
      this.router,
      this.permissionsService
    );
    this._state$.next(builder.createState(profileStatus));
  }

  private getAlerts(profileStatus: ProfileStatus): ProfileStatusAlert[] {
    return profileStatus.alerts.map((alert) => {
      switch (alert) {
        case AlertCode.PLR_BAD_STANDING:
          return {
            heading: 'There is a problem with your college licence',
            content: 'Contact your college for more information.',
          };
        case AlertCode.TRANSIENT_ERROR:
          return {
            heading: 'Having trouble verifying your college licence?',
            content: `Your licence may not be active yet. Try again in 24 hours. If this problem persists, contact your college.`,
          };
      }
    });
  }

  // TODO won't scale and needs revision with more than one enrolment for provision
  private hasCompletedProfile(profileStatus: ProfileStatus): boolean {
    const status = profileStatus.status;

    return (
      status.demographics.statusCode === StatusCode.COMPLETED &&
      // TODO won't work when college is hidden (ie. PHSA authenticated users)
      status.collegeCertification?.statusCode === StatusCode.COMPLETED &&
      status.saEforms?.statusCode === StatusCode.AVAILABLE
    );
  }

  private clearState(): void {
    this._profileStatus = null;
    this._state$.next(null);
    this._completedProfile = false;
    this._alerts = [];
  }
}
