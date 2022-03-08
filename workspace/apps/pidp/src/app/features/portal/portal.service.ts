import { Injectable } from '@angular/core';
import { Router } from '@angular/router';

import { BehaviorSubject, Observable } from 'rxjs';

import { AccessRequestResource } from '@app/core/resources/access-request-resource.service';
import { PartyService } from '@app/core/services/party.service';
import { ProfileStatus } from '@app/features/portal/models/profile-status.model';

import { AlertCode } from './enums/alert-code.enum';
import { StatusCode } from './enums/status-code.enum';
import {
  CollegeCertificationPortalSection,
  DemographicsPortalSection,
  HcimWebEnrolmentPortalSection,
  IPortalSection,
  SaEformsPortalSection,
  SignedAcceptedDocumentsPortalSection,
} from './models/portal-section';
import { ProfileStatusAlert } from './models/profile-status-alert.model';

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
  private _acceptedCollectionNotice: boolean;
  private _completedProfile: boolean;
  private _state$: BehaviorSubject<Record<string, IPortalSection[]>>;

  public constructor(
    private router: Router,
    private partyService: PartyService,
    private accessRequestResource: AccessRequestResource
  ) {
    this._profileStatus = null;
    this._acceptedCollectionNotice = false;
    this._state$ = new BehaviorSubject<Record<string, IPortalSection[]>>({});
    this._completedProfile = false;
    this._alerts = [];
  }

  public get state$(): Observable<Record<string, IPortalSection[]>> {
    return this._state$.asObservable();
  }

  public get acceptedCollectionNotice(): boolean {
    return this._acceptedCollectionNotice;
  }

  public set acceptedCollectionNotice(hasAccepted: boolean) {
    this._acceptedCollectionNotice = hasAccepted;
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
      this._profileStatus?.status.collegeCertification.statusCode ===
      StatusCode.ERROR
    );
  }

  public updateState(profileStatus: ProfileStatus | null): void {
    if (!profileStatus) {
      // TODO clear service when this occurs
      return;
    }

    this._profileStatus = profileStatus;
    this._alerts = this.getAlerts(profileStatus);
    this._completedProfile = this.hasCompletedProfile(profileStatus);

    this._state$.next({
      profileIdentitySections: this.getProfileIdentitySections(profileStatus),
      accessToSystemsSections: this.getAccessToSystemsSections(profileStatus),
      yourDocumentsSections: this.getYourDocumentsSections(),
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

  private hasCompletedProfile(profileStatus: ProfileStatus): boolean {
    // TODO won't scale and needs revision with more than one enrolment for provision
    const status = profileStatus.status;

    return (
      status.demographics.statusCode === StatusCode.COMPLETED &&
      status.collegeCertification.statusCode === StatusCode.COMPLETED &&
      status.saEforms.statusCode === StatusCode.AVAILABLE
    );
  }

  private getProfileIdentitySections(
    profileStatus: ProfileStatus
  ): IPortalSection[] {
    return [
      new DemographicsPortalSection(profileStatus, this.router),
      new CollegeCertificationPortalSection(profileStatus, this.router),
    ];
  }

  private getAccessToSystemsSections(
    profileStatus: ProfileStatus
  ): IPortalSection[] {
    return [
      new SaEformsPortalSection(
        profileStatus,
        this.router,
        this.partyService,
        this.accessRequestResource
      ),
      // TODO uncomment when available through API
      // new HcimWebEnrolmentPortalSection(profileStatus, this.router),
    ];
  }

  private getYourDocumentsSections(): IPortalSection[] {
    return [new SignedAcceptedDocumentsPortalSection(this.router)];
  }
}
