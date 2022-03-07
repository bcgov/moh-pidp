import { Injectable } from '@angular/core';

import { BehaviorSubject, Observable } from 'rxjs';

import { AccessRoutes } from '@app/features/access/access.routes';
import {
  PortalSection,
  PortalSectionType,
} from '@app/features/portal/models/portal-section.model';
import {
  AlertCode,
  ProfileStatus,
  ProfileStatusAlert,
  StatusCode,
} from '@app/features/portal/models/profile-status.model';
import { ProfileRoutes } from '@app/features/profile/profile.routes';
import { YourProfileRoutes } from '@app/features/your-profile/your-profile.routes';

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
  private _state$: BehaviorSubject<Record<string, PortalSection[]>>;

  public constructor() {
    this._profileStatus = null;
    this._acceptedCollectionNotice = false;
    this._state$ = new BehaviorSubject<Record<string, PortalSection[]>>({});
    this._completedProfile = false;
    this._alerts = [];
  }

  public get state$(): Observable<Record<string, PortalSection[]>> {
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
    this._alerts = profileStatus.alerts.map((alert) => {
      switch (alert) {
        case AlertCode.PLR_BAD_STANDING:
          return {
            heading: 'There is a problem with your college licence',
            content: 'Contact your college for more information.',
          };
        case AlertCode.TRANSIENT_ERROR:
          return {
            heading: 'Having trouble verifying your college licence?',
            content:
              'Your licence may not be active yet. Try again in 24 hours. If this problem persists, contact your college.',
          };
      }
    });
    // TODO won't scale but works with one system to provision
    // TODO should be loop over profile keys and have at least one access request
    this._completedProfile =
      profileStatus?.status.demographics.statusCode === StatusCode.COMPLETED &&
      profileStatus?.status.collegeCertification.statusCode ===
        StatusCode.COMPLETED &&
      profileStatus.status.saEforms.statusCode === StatusCode.AVAILABLE;

    this._state$.next({
      profileIdentitySections: this.getProfileIdentitySections(profileStatus),
      accessToSystemsSections: this.getAccessToSystemsSections(profileStatus),
      yourProfileSections: this.getYourProfileSections(),
    });
  }

  private getProfileIdentitySections(
    profileStatus: ProfileStatus
  ): PortalSection[] {
    const demographics = profileStatus.status.demographics;
    const demographicsStatusCode = demographics.statusCode;
    const collegeCertification = profileStatus.status.collegeCertification;
    const collegeCertificationStatusCode =
      profileStatus.status.collegeCertification.statusCode;

    return [
      {
        icon: 'fingerprint',
        // TODO swap for known key that now exists in the response
        type: PortalSectionType.PERSONAL_INFORMATION,
        heading: 'Personal Information',
        hint:
          demographicsStatusCode === StatusCode.ERROR ||
          demographicsStatusCode === StatusCode.COMPLETED
            ? ''
            : '1 min to complete',
        description: 'Personal and Contact Information',
        properties:
          demographicsStatusCode === StatusCode.COMPLETED
            ? [
                {
                  key: 'fullName',
                  value: `${demographics.firstName} ${demographics.lastName}`,
                },
                {
                  key: 'email',
                  value: demographics.email,
                },
                {
                  key: 'phone',
                  value: demographics.phone,
                },
              ]
            : [],
        action: {
          label: 'Update',
          route: ProfileRoutes.routePath(ProfileRoutes.PERSONAL_INFO_PAGE),
          disabled:
            demographicsStatusCode === StatusCode.ERROR ||
            demographicsStatusCode === StatusCode.NOT_AVAILABLE,
        },
        statusType:
          demographicsStatusCode === StatusCode.ERROR
            ? 'danger'
            : demographicsStatusCode === StatusCode.COMPLETED
            ? 'success'
            : 'warn',
        status:
          demographicsStatusCode === StatusCode.ERROR
            ? ''
            : demographicsStatusCode === StatusCode.COMPLETED
            ? 'Completed'
            : 'Incomplete',
      },
      {
        icon: 'fingerprint',
        type: PortalSectionType.COLLEGE_LICENCE_INFORMATION,
        heading: 'College Licence Information',
        hint:
          collegeCertificationStatusCode === StatusCode.ERROR ||
          collegeCertificationStatusCode === StatusCode.COMPLETED
            ? ''
            : '1 min to complete',
        description: 'College Licence Information and Validation',
        properties:
          collegeCertificationStatusCode === StatusCode.ERROR ||
          collegeCertificationStatusCode === StatusCode.COMPLETED
            ? [
                {
                  key: 'collegeCode',
                  value: collegeCertification.collegeCode,
                },
                {
                  key: 'licenceNumber',
                  value: collegeCertification.licenceNumber,
                  label: 'College Licence Number:',
                },
                {
                  key: 'status',
                  value:
                    collegeCertificationStatusCode !== StatusCode.ERROR &&
                    demographicsStatusCode === StatusCode.COMPLETED
                      ? 'Verified'
                      : 'Not Verified',
                  label: 'College Licence Status:',
                },
              ]
            : [],
        action: {
          label: 'Update',
          route: ProfileRoutes.routePath(
            ProfileRoutes.COLLEGE_LICENCE_INFO_PAGE
          ),
          disabled:
            demographicsStatusCode !== StatusCode.COMPLETED ||
            collegeCertificationStatusCode === StatusCode.NOT_AVAILABLE,
        },
        statusType:
          collegeCertificationStatusCode === StatusCode.ERROR
            ? 'danger'
            : collegeCertificationStatusCode === StatusCode.COMPLETED
            ? 'success'
            : 'warn',
        status:
          collegeCertificationStatusCode === StatusCode.ERROR
            ? 'Licence validation error'
            : collegeCertificationStatusCode === StatusCode.COMPLETED
            ? 'Completed'
            : 'Incomplete',
      },
    ];
  }

  private getAccessToSystemsSections(
    profileStatus: ProfileStatus
  ): PortalSection[] {
    const saEformsStatusCode = profileStatus.status.saEforms.statusCode;
    const demographicsStatusCode = profileStatus.status.demographics.statusCode;
    const collegeCertStatusCode =
      profileStatus.status.collegeCertification.statusCode;
    return [
      {
        icon: 'fingerprint',
        type: PortalSectionType.SA_EFORMS,
        heading: 'Special Authority eForms',
        description: `Enrol here for access to PharmaCare's Special Authority eForms application.`,
        action: {
          label:
            saEformsStatusCode === StatusCode.COMPLETED ? 'View' : 'Request',
          route: AccessRoutes.routePath(AccessRoutes.SPECIAL_AUTH_EFORMS_PAGE),
          disabled: !(
            demographicsStatusCode === StatusCode.COMPLETED &&
            collegeCertStatusCode === StatusCode.COMPLETED
          ),
        },
        statusType:
          saEformsStatusCode === StatusCode.COMPLETED ? 'success' : 'info',
        status:
          saEformsStatusCode === StatusCode.AVAILABLE
            ? 'You are eligible to use Special Authority eForms'
            : saEformsStatusCode === StatusCode.COMPLETED
            ? 'Completed'
            : '',
      },
    ];
  }

  private getYourProfileSections(): PortalSection[] {
    return [
      {
        icon: 'fingerprint',
        type: PortalSectionType.SIGNED_ACCEPTED_DOCUMENTS,
        heading: 'View Signed or Accepted Documents',
        description: 'View Agreement(s)',
        action: {
          label: 'View',
          route: YourProfileRoutes.routePath(
            YourProfileRoutes.SIGNED_ACCEPTED_DOCUMENTS_PAGE
          ),
          disabled: false,
        },
      },
    ];
  }
}
