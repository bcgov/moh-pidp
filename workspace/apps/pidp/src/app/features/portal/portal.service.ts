import { Injectable } from '@angular/core';
import { Router } from '@angular/router';

import { BehaviorSubject, Observable } from 'rxjs';

import { ArrayUtils } from '@bcgov/shared/utils';

import { PermissionsService } from '@app/modules/permissions/permissions.service';
import { Role } from '@app/shared/enums/roles.enum';

import { AlertCode } from './enums/alert-code.enum';
import { StatusCode } from './enums/status-code.enum';
import {
  CollegeCertificationPortalSection,
  DemographicsPortalSection,
  HcimReenrolmentPortalSection,
  IPortalSection,
  PortalSectionStatusKey,
  SaEformsPortalSection,
  SignedAcceptedDocumentsPortalSection,
} from './sections/classes';
import { ComplianceTrainingPortalSection } from './sections/classes/compliance-training-portal-section.class';
import { SitePrivacySecurityPortalSection } from './sections/classes/site-privacy-security-checklist-portal-section.class';
import { TransactionsPortalSection } from './sections/classes/transactions-portal-section.class';
import { UserAccessAgreementPortalSection } from './sections/classes/user-access-agreement-portal-section.class';
import { ProfileStatusAlert } from './sections/models/profile-status-alert.model';
import { ProfileStatus } from './sections/models/profile-status.model';

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
  private _state$: BehaviorSubject<Record<string, IPortalSection[]>>;

  public constructor(
    private router: Router,
    private permissionsService: PermissionsService
  ) {
    this._profileStatus = null;
    this._state$ = new BehaviorSubject<Record<string, IPortalSection[]>>({});
    this._completedProfile = false;
    this._alerts = [];
  }

  public get state$(): Observable<Record<string, IPortalSection[]>> {
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

  public updateState(profileStatus: ProfileStatus | null): void {
    if (!profileStatus) {
      this.clearState();
      return;
    }

    this._profileStatus = profileStatus;
    this._alerts = this.getAlerts(profileStatus);
    this._completedProfile = this.hasCompletedProfile(profileStatus);

    this._state$.next({
      profileIdentitySections: this.getProfileIdentitySections(profileStatus),
      accessSystemsSections: this.getAccessSystemsSections(profileStatus),
      trainingSections: this.getTrainingSections(profileStatus),
      documentsSections: this.getDocumentsSections(),
    });
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
      (status.hcim?.statusCode === StatusCode.AVAILABLE ||
        (status.collegeCertification?.statusCode === StatusCode.COMPLETED &&
          status.saEforms?.statusCode === StatusCode.AVAILABLE))
    );
  }

  // TODO loop through keys in preferred order and insert using factory based on status
  private getProfileIdentitySections(
    profileStatus: ProfileStatus
  ): IPortalSection[] {
    return [
      new DemographicsPortalSection(profileStatus, this.router),
      ...ArrayUtils.insertResultIf<IPortalSection>(
        this.insertSection('collegeCertification', profileStatus),
        () => [
          new CollegeCertificationPortalSection(profileStatus, this.router),
        ]
      ),
      ...ArrayUtils.insertResultIf<IPortalSection>(
        this.permissionsService.hasRole([
          Role.FEATURE_PIDP_DEMO,
          Role.FEATURE_AMH_DEMO,
        ]),
        () => [new UserAccessAgreementPortalSection(profileStatus, this.router)]
      ),
    ];
  }

  // TODO loop through keys in preferred order and insert using factory based on status
  private getAccessSystemsSections(
    profileStatus: ProfileStatus
  ): IPortalSection[] {
    return [
      ...ArrayUtils.insertResultIf<IPortalSection>(
        this.insertSection('saEforms', profileStatus),
        () => [new SaEformsPortalSection(profileStatus, this.router)]
      ),
      ...ArrayUtils.insertResultIf<IPortalSection>(
        this.insertSection('hcim', profileStatus),
        () => [new HcimReenrolmentPortalSection(profileStatus, this.router)]
      ),
      ...ArrayUtils.insertResultIf<IPortalSection>(
        this.permissionsService.hasRole([
          Role.FEATURE_PIDP_DEMO,
          Role.FEATURE_AMH_DEMO,
        ]),
        () => [new SitePrivacySecurityPortalSection(profileStatus, this.router)]
      ),
    ];
  }

  // TODO loop through keys in preferred order and insert using factory based on status
  private getTrainingSections(profileStatus: ProfileStatus): IPortalSection[] {
    return [
      ...ArrayUtils.insertResultIf<IPortalSection>(
        this.permissionsService.hasRole([
          Role.FEATURE_PIDP_DEMO,
          Role.FEATURE_AMH_DEMO,
        ]),
        () => [new ComplianceTrainingPortalSection(profileStatus, this.router)]
      ),
    ];
  }

  // TODO loop through keys in preferred order and insert using factory based on status
  private getDocumentsSections(): IPortalSection[] {
    return [
      new SignedAcceptedDocumentsPortalSection(this.router),
      ...ArrayUtils.insertResultIf<IPortalSection>(
        this.permissionsService.hasRole(Role.FEATURE_PIDP_DEMO),
        () => [new TransactionsPortalSection(this.router)]
      ),
    ];
  }

  private insertSection(
    portalSectionKey: PortalSectionStatusKey,
    profileStatus: ProfileStatus
  ): boolean {
    return (
      profileStatus.status[portalSectionKey].statusCode !== StatusCode.HIDDEN
    );
  }

  private clearState(): void {
    this._profileStatus = null;
    this._state$.next({});
    this._completedProfile = false;
    this._alerts = [];
  }
}
