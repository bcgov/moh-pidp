import { Injectable } from '@angular/core';

import { BehaviorSubject, Observable } from 'rxjs';

import { AccessRoutes } from '@app/features/access/access.routes';
import {
  AccessRequestType,
  PortalSection,
  PortalSectionType,
  ProfileAccessType,
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
  private _profileStatus: ProfileStatus | null;
  private _acceptedCollectionNotice: boolean;
  private _completedProfile: boolean;
  private _state$: BehaviorSubject<Record<string, PortalSection[]>>;

  public constructor() {
    this._profileStatus = null;
    this._acceptedCollectionNotice = false;
    this._state$ = new BehaviorSubject<Record<string, PortalSection[]>>({});
    this._completedProfile = false;
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

  // TODO instantiate ProfileStatus and add helper methods to avoid === hell
  public updateState(profileStatus: ProfileStatus | null): void {
    if (!profileStatus) {
      return;
    }

    this._profileStatus = profileStatus;
    this._completedProfile =
      profileStatus.status?.demographics.code === StatusCode.COMPLETED &&
      profileStatus.status?.collegeCertification.code === StatusCode.COMPLETED;

    this._state$.next({
      profileIdentitySections: this.getProfileIdentitySections(profileStatus),
      accessToSystemsSections: this.getAccessToSystemsSections(profileStatus),
      yourProfileSections: this.getYourProfileSections(),
    });
  }

  private getProfileIdentitySections(
    profileStatus?: ProfileStatus | null
  ): PortalSection[] {
    return [
      {
        icon: 'fingerprint',
        type: PortalSectionType.PERSONAL_INFORMATION,
        heading: 'Personal Information',
        hint:
          profileStatus?.status?.demographics.code === StatusCode.COMPLETED
            ? ''
            : '1 min to complete',
        description: 'Personal and Contact Information',
        properties:
          profileStatus?.status?.demographics.code === StatusCode.COMPLETED
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
        actions: [
          {
            label: 'Update',
            disabled: false,
          },
        ],
        route: ProfileRoutes.routePath(ProfileRoutes.PERSONAL_INFO_PAGE),
        statusType:
          profileStatus?.status?.demographics.code === StatusCode.COMPLETED
            ? 'success'
            : 'warn',
        status:
          profileStatus?.status?.demographics.code === StatusCode.COMPLETED
            ? 'completed'
            : 'incomplete',
      },
      {
        icon: 'fingerprint',
        type: PortalSectionType.COLLEGE_LICENCE_INFORMATION,
        heading: 'College Licence Information',
        hint:
          profileStatus?.status?.collegeCertification.code ===
          StatusCode.COMPLETED
            ? ''
            : '1 min to complete',
        description: 'College Licence Information and Validation',
        properties:
          profileStatus?.status?.collegeCertification.code ===
          StatusCode.COMPLETED
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
                    profileStatus?.status?.demographics.code ===
                    StatusCode.COMPLETED
                      ? 'Verified'
                      : 'Not Verified',
                  label: 'Status:',
                },
              ]
            : [],
        actions: [
          {
            label: 'Update',
            disabled: !(
              profileStatus?.status?.demographics.code === StatusCode.COMPLETED
            ),
          },
        ],
        route: ProfileRoutes.routePath(ProfileRoutes.COLLEGE_LICENCE_INFO_PAGE),
        statusType:
          profileStatus?.status?.collegeCertification.code ===
          StatusCode.COMPLETED
            ? 'success'
            : 'warn',
        status:
          profileStatus?.status?.collegeCertification.code ===
          StatusCode.COMPLETED
            ? 'completed'
            : 'incomplete',
      },
    ];
  }

  private getAccessToSystemsSections(
    profileStatus?: ProfileStatus | null
  ): PortalSection[] {
    return [
      {
        icon: 'fingerprint',
        type: AccessRequestType.SA_EFORMS,
        heading: 'Special Authority eForms',
        description: `Enrol here for access to PharmaCare's Special Authority eForms application.`,
        route: AccessRoutes.routePath(AccessRoutes.SPECIAL_AUTH_EFORMS_PAGE),
        statusType: 'info',
        actions: [
          {
            label: 'Request',
            disabled: !(
              profileStatus?.status?.demographics.code ===
                StatusCode.COMPLETED &&
              profileStatus?.status?.collegeCertification.code ===
                StatusCode.COMPLETED
            ),
          },
        ],
      },
    ];
  }

  private getYourProfileSections(): PortalSection[] {
    return [
      {
        icon: 'fingerprint',
        type: ProfileAccessType.SIGNED_ACCEPTED_DOCUMENTS,
        heading: 'View Signed or Accepted Documents',
        description: 'View Agreement(s)',
        route: YourProfileRoutes.routePath(
          YourProfileRoutes.SIGNED_ACCEPTED_DOCUMENTS_PAGE
        ),
        actions: [
          {
            label: 'View',
            disabled: false,
          },
        ],
      },
    ];
  }
}
