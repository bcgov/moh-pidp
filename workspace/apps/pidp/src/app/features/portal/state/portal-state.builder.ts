import { Router } from '@angular/router';

import { ArrayUtils } from '@bcgov/shared/utils';

import { PermissionsService } from '@app/modules/permissions/permissions.service';
import { Role } from '@app/shared/enums/roles.enum';

import { StatusCode } from '../enums/status-code.enum';
import { ProfileStatus } from '../models/profile-status.model';
import { DriverFitnessPortalSection } from './access/driver-fitness-portal-section.class';
import { HcimAccountTransferPortalSection } from './access/hcim-account-transfer-portal-section.class';
import { HcimEnrolmentPortalSection } from './access/hcim-enrolment-portal-section.class';
import { SaEformsPortalSection } from './access/sa-eforms-portal-section.class';
import { SitePrivacySecurityPortalSection } from './access/site-privacy-security-checklist-portal-section.class';
import { SignedAcceptedDocumentsPortalSection } from './history/signed-accepted-documents-portal-section.class';
import { TransactionsPortalSection } from './history/transactions-portal-section.class';
import { AdministratorInfoPortalSection } from './organization/administrator-information-portal-section';
import { FacilityDetailsPortalSection } from './organization/facility-details-portal-section.class';
import { OrganizationDetailsPortalSection } from './organization/organization-details-portal-section.class';
import { PortalSectionStatusKey } from './portal-section-status-key.type';
import { IPortalSection } from './portal-section.model';
import { CollegeCertificationPortalSection } from './profile/college-certification-portal-section.class';
import { DemographicsPortalSection } from './profile/demographics-portal-section.class';
import { UserAccessAgreementPortalSection } from './profile/user-access-agreement-portal-section.class';
import { ComplianceTrainingPortalSection } from './training/compliance-training-portal-section.class';

/**
 * @description
 * Group keys as a readonly tuple to allow iteration
 * at runtime.
 */
export const portalStateGroupKeys = [
  'profile',
  'access',
  'organization',
  'training',
  'history',
] as const;

/**
 * @description
 * Union of keys generated from the tuple.
 */
export type PortalStateGroupKey = typeof portalStateGroupKeys[number];

export type PortalState = Record<PortalStateGroupKey, IPortalSection[]> | null;

export class PortalStateBuilder {
  public constructor(
    private router: Router,
    private permissionsService: PermissionsService
  ) {}

  public createState(
    profileStatus: ProfileStatus
  ): Record<PortalStateGroupKey, IPortalSection[]> {
    return {
      profile: this.createProfileGroup(profileStatus),
      access: this.createAccessGroup(profileStatus),
      organization: this.createOrganizationGroup(profileStatus),
      training: this.createTrainingGroup(profileStatus),
      history: this.createHistoryGroup(),
    };
  }

  // TODO see where the next few enrolments lead and then drop these methods
  // for building out the portal state using factories, but premature
  // optimization until more is known

  private createProfileGroup(profileStatus: ProfileStatus): IPortalSection[] {
    return [
      new DemographicsPortalSection(profileStatus, this.router),
      ...ArrayUtils.insertResultIf<IPortalSection>(
        this.insertSection('collegeCertification', profileStatus),
        () => [
          new CollegeCertificationPortalSection(profileStatus, this.router),
        ]
      ),
      ...ArrayUtils.insertResultIf<IPortalSection>(
        // TODO remove permissions when API exists and ready for production, or
        // TODO replace || with && to keep it flagged when API exists
        this.insertSection('userAccessAgreement', profileStatus) ||
          this.permissionsService.hasRole([
            Role.FEATURE_PIDP_DEMO,
            Role.FEATURE_AMH_DEMO,
          ]),
        () => [new UserAccessAgreementPortalSection(profileStatus, this.router)]
      ),
    ];
  }

  private createOrganizationGroup(
    profileStatus: ProfileStatus
  ): IPortalSection[] {
    return [
      ...ArrayUtils.insertResultIf<IPortalSection>(
        // TODO remove permissions when API exists and ready for production, or
        // TODO replace || with && to keep it flagged when API exists
        this.insertSection('organizationDetails', profileStatus) ||
          this.permissionsService.hasRole([Role.FEATURE_PIDP_DEMO]),
        () => [new OrganizationDetailsPortalSection(profileStatus, this.router)]
      ),
      ...ArrayUtils.insertResultIf<IPortalSection>(
        // TODO remove permissions when API exists and ready for production, or
        // TODO replace || with && to keep it flagged when API exists
        this.insertSection('facilityDetails', profileStatus) ||
          this.permissionsService.hasRole([Role.FEATURE_PIDP_DEMO]),
        () => [new FacilityDetailsPortalSection(profileStatus, this.router)]
      ),
      ...ArrayUtils.insertResultIf<IPortalSection>(
        // TODO remove permissions when API exists and ready for production, or
        // TODO replace || with && to keep it flagged when API exists
        this.insertSection('administratorInfo', profileStatus) ||
          this.permissionsService.hasRole([Role.FEATURE_PIDP_DEMO]),
        () => [new AdministratorInfoPortalSection(profileStatus, this.router)]
      ),
    ];
  }

  private createAccessGroup(profileStatus: ProfileStatus): IPortalSection[] {
    return [
      ...ArrayUtils.insertResultIf<IPortalSection>(
        this.insertSection('saEforms', profileStatus),
        () => [new SaEformsPortalSection(profileStatus, this.router)]
      ),
      ...ArrayUtils.insertResultIf<IPortalSection>(
        this.insertSection('hcimAccountTransfer', profileStatus),
        () => [new HcimAccountTransferPortalSection(profileStatus, this.router)]
      ),
      ...ArrayUtils.insertResultIf<IPortalSection>(
        // TODO remove permissions when ready for production
        this.insertSection('hcimEnrolment', profileStatus) &&
          this.permissionsService.hasRole([Role.FEATURE_PIDP_DEMO]),
        () => [new HcimEnrolmentPortalSection(profileStatus, this.router)]
      ),
      ...ArrayUtils.insertResultIf<IPortalSection>(
        // TODO remove permissions when API exists and ready for production, or
        // TODO replace || with && to keep it flagged when API exists
        this.insertSection('sitePrivacySecurityChecklist', profileStatus) ||
          this.permissionsService.hasRole([Role.FEATURE_PIDP_DEMO]),
        () => [new SitePrivacySecurityPortalSection(profileStatus, this.router)]
      ),
      ...ArrayUtils.insertResultIf<IPortalSection>(
        // TODO remove permissions when ready for production
        this.permissionsService.hasRole([Role.FEATURE_PIDP_DEMO]) &&
          this.insertSection('driverFitness', profileStatus),
        () => [new DriverFitnessPortalSection(profileStatus, this.router)]
      ),
    ];
  }

  private createTrainingGroup(profileStatus: ProfileStatus): IPortalSection[] {
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

  private createHistoryGroup(): IPortalSection[] {
    return [
      new SignedAcceptedDocumentsPortalSection(this.router),
      new TransactionsPortalSection(this.router),
    ];
  }

  private insertSection(
    portalSectionKey: PortalSectionStatusKey,
    profileStatus: ProfileStatus
  ): boolean {
    const statusCode = profileStatus.status[portalSectionKey]?.statusCode;
    return statusCode && statusCode !== StatusCode.HIDDEN;
  }
}
