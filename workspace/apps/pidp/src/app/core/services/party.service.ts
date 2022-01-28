import { Injectable } from '@angular/core';

import { BehaviorSubject, Observable } from 'rxjs';

import { AccessRoutes } from '@app/features/access/access.routes';
import { PortalSection } from '@app/features/portal/models/portal-section.model';
import { ProfileRoutes } from '@app/features/profile/profile.routes';
import { YourProfileRoutes } from '@app/features/your-profile/your-profile.routes';
import { LookupCodePipe } from '@app/modules/lookup/lookup-code.pipe';

import { ProfileStatus } from '../resources/party-resource.service';

// TODO create custom initializer that inits keycloak then gets partyId and stores in this service for easy replacement by state management
@Injectable({
  providedIn: 'root',
})
export class PartyService {
  private _profileStatus: ProfileStatus | null;
  private _acceptedCollectionNotice: boolean;
  private _completedProfile: boolean;
  private _state$: BehaviorSubject<Record<string, PortalSection[]>>;

  public constructor(private lookupCodePipe: LookupCodePipe) {
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

  public updateState(profileStatus: ProfileStatus | null): void {
    if (!profileStatus) {
      return;
    }

    this._profileStatus = profileStatus;
    this._completedProfile =
      profileStatus.demographicsComplete &&
      profileStatus.collegeCertificationComplete;

    this._state$.next({
      profileIdentitySections: this.getProfileIdentitySections(profileStatus),
      accessToSystemsSections: this.getAccessToSystemsSections(profileStatus),
      yourProfileSections: this.getYourProfileSections(profileStatus),
    });
  }

  private getProfileIdentitySections(
    profileStatus?: ProfileStatus | null
  ): PortalSection[] {
    return [
      {
        icon: 'fingerprint',
        type: 'personal-information',
        title: 'Personal Information',
        hint: profileStatus?.demographicsComplete ? '' : '1 min to complete',
        description: 'Personal and Contact Information',
        properties: profileStatus?.demographicsComplete
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
        actionLabel: 'Update',
        route: ProfileRoutes.routePath(ProfileRoutes.PERSONAL_INFO_PAGE),
        statusType: profileStatus?.demographicsComplete ? 'success' : 'warn',
        status: profileStatus?.demographicsComplete
          ? 'completed'
          : 'incomplete',
        actionDisabled: false,
      },
      {
        icon: 'fingerprint',
        type: 'college-licence-information',
        title: 'College Licence Information',
        hint: profileStatus?.collegeCertificationComplete
          ? ''
          : '1 min to complete',
        description: 'College Licence Information and Validation',
        properties: profileStatus?.collegeCertificationComplete
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
                // TODO indicate whether they are verified or not
                value: '-',
                label: 'Status:',
              },
            ]
          : [],
        actionLabel: 'Update',
        route: ProfileRoutes.routePath(ProfileRoutes.COLLEGE_LICENCE_INFO_PAGE),
        statusType: profileStatus?.collegeCertificationComplete
          ? 'success'
          : 'warn',
        status: profileStatus?.collegeCertificationComplete
          ? 'completed'
          : 'incomplete',
        actionDisabled: !profileStatus?.demographicsComplete,
      },
    ];
  }

  private getAccessToSystemsSections(
    profileStatus?: ProfileStatus | null
  ): PortalSection[] {
    return [
      {
        icon: 'fingerprint',
        type: 'special-authority-eforms',
        title: 'Special Authority eForms',
        description: `Enrol here for access to PharmaCare's Special Authority eForms application.`,
        route: AccessRoutes.routePath(AccessRoutes.SPECIAL_AUTH_EFORMS_PAGE),
        statusType: 'info',
        actionLabel: 'Request',
        actionDisabled: !(
          profileStatus?.demographicsComplete &&
          profileStatus?.collegeCertificationComplete
        ),
      },
    ];
  }

  private getYourProfileSections(
    profileStatus?: ProfileStatus | null
  ): PortalSection[] {
    return [
      {
        icon: 'fingerprint',
        type: 'view-signed-or-accepted-documents',
        title: 'View Signed or Accepted Documents',
        description: 'View Agreement(s)',
        actionLabel: 'View',
        route: YourProfileRoutes.routePath(
          YourProfileRoutes.SIGNED_ACCEPTED_DOCUMENTS_PAGE
        ),
        actionDisabled: !(
          profileStatus?.demographicsComplete &&
          profileStatus?.collegeCertificationComplete
        ),
      },
    ];
  }
}
