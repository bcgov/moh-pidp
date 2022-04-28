import { Router } from '@angular/router';

import { ArrayUtils } from '@bcgov/shared/utils';

import { PermissionsService } from '@app/modules/permissions/permissions.service';
import { Role } from '@app/shared/enums/roles.enum';

import { StatusCode } from '../enums/status-code.enum';
import { ProfileStatus } from '../models/profile-status.model';
import { HcimAccountTransferPortalSection } from './access/hcim-account-transfer-portal-section.class';
import { HcimEnrolmentPortalSection } from './access/hcim-enrolment-portal-section.class';
import { SaEformsPortalSection } from './access/sa-eforms-portal-section.class';
import { SitePrivacySecurityPortalSection } from './access/site-privacy-security-checklist-portal-section.class';
import { SignedAcceptedDocumentsPortalSection } from './documents/signed-accepted-documents-portal-section.class';
import { TransactionsPortalSection } from './documents/transactions-portal-section.class';
import { PortalSectionStatusKey } from './portal-section-status-key.type';
import { IPortalSection } from './portal-section.model';
import { CollegeCertificationPortalSection } from './profile/college-certification-portal-section.class';
import { DemographicsPortalSection } from './profile/demographics-portal-section.class';
import { UserAccessAgreementPortalSection } from './profile/user-access-agreement-portal-section.class';
import { ComplianceTrainingPortalSection } from './training/compliance-training-portal-section.class';

/**
 * @description
 * List of portal state group keys as a readonly tuple
 * to allow iteration and use of keyof at runtime.
 */
export const portalStateGroupKeys = [
  'profile',
  'access',
  'training',
  'documents',
] as const;

/**
 * @description
 * Portal state group keys as a union.
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
      training: this.createTrainingGroup(profileStatus),
      documents: this.createDocumentsGroup(),
    };
  }

  // TODO see where the next enrolments lead and then drop these methods
  // for building out the portal state and create classes, but premature
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
        // TODO remove demo permissions when API exists
        this.permissionsService.hasRole([
          Role.FEATURE_PIDP_DEMO,
          Role.FEATURE_AMH_DEMO,
        ]) || this.insertSection('userAccessAgreement', profileStatus),
        () => [new UserAccessAgreementPortalSection(profileStatus, this.router)]
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
        // TODO remove permissions when API exists
        this.permissionsService.hasRole([Role.FEATURE_PIDP_DEMO]) ||
          this.insertSection('hcimEnrolment', profileStatus),
        () => [new HcimEnrolmentPortalSection(profileStatus, this.router)]
      ),
      ...ArrayUtils.insertResultIf<IPortalSection>(
        // TODO remove permissions when API exists
        this.permissionsService.hasRole([Role.FEATURE_PIDP_DEMO]) ||
          this.insertSection('sitePrivacySecurityChecklist', profileStatus),
        () => [new SitePrivacySecurityPortalSection(profileStatus, this.router)]
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

  private createDocumentsGroup(): IPortalSection[] {
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
    const statusCode = profileStatus.status[portalSectionKey]?.statusCode;
    return statusCode && statusCode !== StatusCode.HIDDEN;
  }
}
