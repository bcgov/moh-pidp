import { Router } from '@angular/router';

import { ArrayUtils } from '@bcgov/shared/utils';

import { PermissionsService } from '@app/modules/permissions/permissions.service';
import { Role } from '@app/shared/enums/roles.enum';

import { StatusCode } from '../enums/status-code.enum';
import { ProfileStatus } from '../models/profile-status.model';
import { IAccessSection } from './access-section.model';
import { BcProviderPortalSection } from './access/bc-provider-portal-section.class';
import { DriverFitnessPortalSection } from './access/driver-fitness-portal-section.class';
import { HcimAccountTransferPortalSection } from './access/hcim-account-transfer-portal-section.class';
import { ImmsBCEformsPortalSection } from './access/immsbc-eforms-portal-section.class';
import { MsTeamsClinicMemberPortalSection } from './access/ms-teams-clinic-member-portal-section.class';
import { MsTeamsPrivacyOfficerPortalSection } from './access/ms-teams-privacy-officer-portal-section.class';
import { PrescriptionRefillEformsPortalSection } from './access/prescription-refill-eforms-portal-section.class';
import { ProviderReportingPortalSection } from './access/provider-reporting-portal-section.class';
import { ProvincialAttachmentSystemPortalSection } from './access/provincial-attachment-system-portal-section.class';
import { SaEformsPortalSection } from './access/sa-eforms-portal-section.class';
import { MfaSetupPortalSection } from './faq/mfa-setup-portal-section.class';
import { SignedAcceptedDocumentsPortalSection } from './history/signed-accepted-documents-portal-section.class';
import { TransactionsPortalSection } from './history/transactions-portal-section.class';
import { EndorsementsPortalSection } from './organization/endorsements-portal-section.class';
import { PortalSectionStatusKey } from './portal-section-status-key.type';
import { IPortalSection } from './portal-section.model';
import { AccountLinkingPortalSection } from './profile/account-linking-portal-section.class';
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
  'faq',
] as const;

/**
 * @description
 * Group keys as a readonly tuple to allow iteration
 * at runtime.
 */
export const accessStateGroupKey = ['access'] as const;

/**
 * @description
 * Union of keys generated from the tuple.
 */
export type PortalStateGroupKey = (typeof portalStateGroupKeys)[number];

/**
 * @description
 * Union of keys generated from the tuple.
 */
export type AccessStateGroupKey = (typeof accessStateGroupKey)[number];

export type AccessState = Record<AccessStateGroupKey, IAccessSection[]> | null;

export type PortalState = Record<PortalStateGroupKey, IPortalSection[]> | null;

export class AccessStateBuilder {
  public constructor(
    private router: Router,
    private permissionsService: PermissionsService,
  ) {}

  public createAccessState(
    profileStatus: ProfileStatus,
  ): Record<AccessStateGroupKey, IAccessSection[]> {
    return {
      access: this.createAccessGroup(profileStatus),
    };
  }

  private createAccessGroup(profileStatus: ProfileStatus): IAccessSection[] {
    return [
      ...ArrayUtils.insertResultIf<IAccessSection>(
        this.insertSection('saEforms', profileStatus),
        () => [new SaEformsPortalSection(profileStatus, this.router)],
      ),
      ...ArrayUtils.insertResultIf<IAccessSection>(
        this.insertSection('prescriptionRefillEforms', profileStatus),
        () => [
          new PrescriptionRefillEformsPortalSection(profileStatus, this.router),
        ],
      ),
      ...ArrayUtils.insertResultIf<IAccessSection>(
        this.insertSection('bcProvider', profileStatus),
        () => [new BcProviderPortalSection(profileStatus, this.router)],
      ),
      ...ArrayUtils.insertResultIf<IAccessSection>(
        this.insertSection('hcimAccountTransfer', profileStatus),
        () => [
          new HcimAccountTransferPortalSection(profileStatus, this.router),
        ],
      ),
      ...ArrayUtils.insertResultIf<IAccessSection>(
        this.insertSection('driverFitness', profileStatus),
        () => [new DriverFitnessPortalSection(profileStatus, this.router)],
      ),
      ...ArrayUtils.insertResultIf<IAccessSection>(
        this.insertSection('msTeamsPrivacyOfficer', profileStatus),
        () => [
          new MsTeamsPrivacyOfficerPortalSection(profileStatus, this.router),
        ],
      ),
      ...ArrayUtils.insertResultIf<IAccessSection>(
        this.insertSection('msTeamsClinicMember', profileStatus),
        () => [
          new MsTeamsClinicMemberPortalSection(profileStatus, this.router),
        ],
      ),
      ...ArrayUtils.insertResultIf<IAccessSection>(
        // TODO remove permissions when ready for production
        this.insertSection('providerReportingPortal', profileStatus) &&
          this.permissionsService.hasRole([Role.FEATURE_PIDP_DEMO]),
        () => [new ProviderReportingPortalSection(profileStatus, this.router)],
      ),
      ...ArrayUtils.insertResultIf<IAccessSection>(
        this.insertSection('provincialAttachmentSystem', profileStatus),
        () => [
          new ProvincialAttachmentSystemPortalSection(
            profileStatus,
            this.router,
          ),
        ],
      ),
      ...ArrayUtils.insertResultIf<IAccessSection>(
        this.insertSection('immsBCEforms', profileStatus),
        () => [new ImmsBCEformsPortalSection(profileStatus, this.router)],
      ),
    ];
  }

  private insertSection(
    portalSectionKey: PortalSectionStatusKey,
    profileStatus: ProfileStatus,
  ): boolean {
    const statusCode = profileStatus.status[portalSectionKey]?.statusCode;
    return statusCode && statusCode !== StatusCode.HIDDEN;
  }
}

export class PortalStateBuilder {
  public constructor(
    private router: Router,
    private permissionsService: PermissionsService,
  ) {}

  public createState(
    profileStatus: ProfileStatus,
  ): Record<PortalStateGroupKey, IPortalSection[]> {
    // TODO move registration into parent module
    return {
      profile: this.createProfileGroup(profileStatus),
      access: this.createAccessGroup(profileStatus),
      organization: this.createOrganizationGroup(profileStatus),
      training: this.createTrainingGroup(profileStatus),
      history: this.createHistoryGroup(),
      faq: this.createFaqGroup(),
    };
  }

  // TODO see where the next few enrolments lead and then drop these methods
  //      for building out the portal state using factories, but premature
  //      optimization until more is known

  // TODO have these be registered from the modules to a service to
  //      reduce the spread of maintenance and updates. For example,
  //      centralize feature flagging into their own modules have
  //      those modules register those artifacts to services

  private createProfileGroup(profileStatus: ProfileStatus): IPortalSection[] {
    return [
      new DemographicsPortalSection(profileStatus, this.router),
      ...ArrayUtils.insertResultIf<IPortalSection>(
        this.insertSection('collegeCertification', profileStatus),
        () => [
          new CollegeCertificationPortalSection(profileStatus, this.router),
        ],
      ),
      ...ArrayUtils.insertResultIf<IPortalSection>(
        this.insertSection('accountLinking', profileStatus),
        () => [new AccountLinkingPortalSection(profileStatus, this.router)],
      ),
      ...ArrayUtils.insertResultIf<IPortalSection>(
        this.insertSection('userAccessAgreement', profileStatus),
        () => [
          new UserAccessAgreementPortalSection(profileStatus, this.router),
        ],
      ),
    ];
  }

  private createOrganizationGroup(
    profileStatus: ProfileStatus,
  ): IPortalSection[] {
    return [
      ...ArrayUtils.insertResultIf<IPortalSection>(
        this.insertSection('endorsements', profileStatus),
        () => [new EndorsementsPortalSection(profileStatus, this.router)],
      ),
    ];
  }

  private createAccessGroup(profileStatus: ProfileStatus): IPortalSection[] {
    return [
      ...ArrayUtils.insertResultIf<IPortalSection>(
        this.insertSection('saEforms', profileStatus),
        () => [new SaEformsPortalSection(profileStatus, this.router)],
      ),
      ...ArrayUtils.insertResultIf<IPortalSection>(
        this.insertSection('prescriptionRefillEforms', profileStatus),
        () => [
          new PrescriptionRefillEformsPortalSection(profileStatus, this.router),
        ],
      ),
      ...ArrayUtils.insertResultIf<IPortalSection>(
        this.insertSection('bcProvider', profileStatus),
        () => [new BcProviderPortalSection(profileStatus, this.router)],
      ),
      ...ArrayUtils.insertResultIf<IPortalSection>(
        this.insertSection('hcimAccountTransfer', profileStatus),
        () => [
          new HcimAccountTransferPortalSection(profileStatus, this.router),
        ],
      ),
      ...ArrayUtils.insertResultIf<IPortalSection>(
        this.insertSection('driverFitness', profileStatus),
        () => [new DriverFitnessPortalSection(profileStatus, this.router)],
      ),
      ...ArrayUtils.insertResultIf<IPortalSection>(
        this.insertSection('msTeamsPrivacyOfficer', profileStatus),
        () => [
          new MsTeamsPrivacyOfficerPortalSection(profileStatus, this.router),
        ],
      ),
      ...ArrayUtils.insertResultIf<IPortalSection>(
        this.insertSection('msTeamsClinicMember', profileStatus),
        () => [
          new MsTeamsClinicMemberPortalSection(profileStatus, this.router),
        ],
      ),
      ...ArrayUtils.insertResultIf<IPortalSection>(
        // TODO remove permissions when ready for production
        this.insertSection('providerReportingPortal', profileStatus) &&
          this.permissionsService.hasRole([Role.FEATURE_PIDP_DEMO]),
        () => [new ProviderReportingPortalSection(profileStatus, this.router)],
      ),
      ...ArrayUtils.insertResultIf<IPortalSection>(
        this.insertSection('provincialAttachmentSystem', profileStatus),
        () => [
          new ProvincialAttachmentSystemPortalSection(
            profileStatus,
            this.router,
          ),
        ],
      ),
      ...ArrayUtils.insertResultIf<IPortalSection>(
        this.insertSection('immsBCEforms', profileStatus),
        () => [new ImmsBCEformsPortalSection(profileStatus, this.router)],
      ),
    ];
  }

  private createTrainingGroup(profileStatus: ProfileStatus): IPortalSection[] {
    return [
      ...ArrayUtils.insertResultIf<IPortalSection>(
        this.permissionsService.hasRole([Role.FEATURE_PIDP_DEMO]),
        () => [new ComplianceTrainingPortalSection(profileStatus, this.router)],
      ),
    ];
  }

  private createHistoryGroup(): IPortalSection[] {
    return [
      new SignedAcceptedDocumentsPortalSection(this.router),
      new TransactionsPortalSection(this.router),
    ];
  }

  private createFaqGroup(): IPortalSection[] {
    return [new MfaSetupPortalSection(this.router)];
  }

  private insertSection(
    portalSectionKey: PortalSectionStatusKey,
    profileStatus: ProfileStatus,
  ): boolean {
    const statusCode = profileStatus.status[portalSectionKey]?.statusCode;
    return statusCode && statusCode !== StatusCode.HIDDEN;
  }
}
