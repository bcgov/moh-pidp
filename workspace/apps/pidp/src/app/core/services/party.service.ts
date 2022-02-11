import { Injectable } from '@angular/core';

import { BehaviorSubject, Observable } from 'rxjs';

import { AccessRoutes } from '@app/features/access/access.routes';
import {
  PortalSection,
  PortalSectionType,
} from '@app/features/portal/models/portal-section.model';
import { ProfileRoutes } from '@app/features/profile/profile.routes';
import { YourProfileRoutes } from '@app/features/your-profile/your-profile.routes';

import { ProfileStatus, StatusCode } from '../resources/party-resource.service';

// TODO create custom initializer that:
//      1. uses existing init for keycloak
//      2. gets partyId and stores in this service
//      3. pulls profile status and stores in this service
@Injectable({
  providedIn: 'root',
})
export class PartyService {
  private _partyId: number | null;
  private _profileStatus: ProfileStatus | null;
  private _acceptedCollectionNotice: boolean;
  private _completedProfile: boolean;
  private _state$: BehaviorSubject<Record<string, PortalSection[]>>;

  public constructor() {
    this._partyId = null;
    this._profileStatus = null;
    this._acceptedCollectionNotice = false;
    this._state$ = new BehaviorSubject<Record<string, PortalSection[]>>({});
    this._completedProfile = false;
  }

  public set partyId(partyId: number | null) {
    this._partyId = partyId;
  }

  public get partyId(): number | null {
    return this._partyId;
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

  public get collegeLicenceValidationError(): boolean {
    return (
      this._profileStatus?.status.collegeCertification.statusCode ===
      StatusCode.ERROR
    );
  }

  // TODO instantiate ProfileStatus and add helper methods to avoid === hell
  public updateState(profileStatus: ProfileStatus | null): void {
    if (!profileStatus) {
      return;
    }

    const status = profileStatus.status;

    this._profileStatus = profileStatus;
    this._completedProfile =
      status?.demographics.statusCode === StatusCode.COMPLETED &&
      status?.collegeCertification.statusCode === StatusCode.COMPLETED &&
      // TODO won't scale but works with one system to provision
      status.saEforms.statusCode === StatusCode.AVAILABLE;

    this._state$.next({
      profileIdentitySections: this.getProfileIdentitySections(profileStatus),
      accessToSystemsSections: this.getAccessToSystemsSections(profileStatus),
      yourProfileSections: this.getYourProfileSections(),
    });
  }

  private getProfileIdentitySections(
    profileStatus: ProfileStatus
  ): PortalSection[] {
    const status = profileStatus.status;
    const demographicsStatusCode = status.demographics.statusCode;
    const collegeCertStatusCode = status.collegeCertification.statusCode;

    return [
      // TODO possibility of an error low, but not handled and will block completely
      {
        icon: 'fingerprint',
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
                  value: `${profileStatus.firstName} ${profileStatus.lastName}`,
                },
                {
                  key: 'email',
                  value: profileStatus.email,
                },
                {
                  key: 'phone',
                  value: profileStatus.phone,
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
          collegeCertStatusCode === StatusCode.ERROR ||
          collegeCertStatusCode === StatusCode.COMPLETED
            ? ''
            : '1 min to complete',
        description: 'College Licence Information and Validation',
        properties:
          // TODO does this error check really make sense?
          collegeCertStatusCode === StatusCode.ERROR ||
          collegeCertStatusCode === StatusCode.COMPLETED
            ? [
                {
                  key: 'collegeCode',
                  value: profileStatus.collegeCode,
                },
                {
                  key: 'licenceNumber',
                  value: profileStatus.licenceNumber,
                  label: 'College Licence Number:',
                },
                {
                  key: 'status',
                  value:
                    collegeCertStatusCode !== StatusCode.ERROR &&
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
            collegeCertStatusCode === StatusCode.NOT_AVAILABLE,
        },
        statusType:
          collegeCertStatusCode === StatusCode.ERROR
            ? 'danger'
            : collegeCertStatusCode === StatusCode.COMPLETED
            ? 'success'
            : 'warn',
        status:
          collegeCertStatusCode === StatusCode.ERROR
            ? 'Licence validation error'
            : collegeCertStatusCode === StatusCode.COMPLETED
            ? 'Completed'
            : 'Incomplete',
      },
    ];
  }

  private getAccessToSystemsSections(
    profileStatus: ProfileStatus
  ): PortalSection[] {
    const status = profileStatus.status;
    const demographicsStatusCode = status.demographics.statusCode;
    const collegeCertStatusCode = status.collegeCertification.statusCode;
    const saEformsStatusCode = status.saEforms.statusCode;
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
