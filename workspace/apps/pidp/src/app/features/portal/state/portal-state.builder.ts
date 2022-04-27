import { Router } from '@angular/router';

import { ArrayUtils } from '@bcgov/shared/utils';

import { PermissionsService } from '@app/modules/permissions/permissions.service';
import { Role } from '@app/shared/enums/roles.enum';

import { StatusCode } from '../enums/status-code.enum';
import {
  CollegeCertificationPortalSection,
  ComplianceTrainingPortalSection,
  DemographicsPortalSection,
  HcimAccountTransferPortalSection,
  HcimEnrolmentPortalSection,
  IPortalSection,
  PortalSectionStatusKey,
  SaEformsPortalSection,
  SignedAcceptedDocumentsPortalSection,
} from '../sections/classes';
import { SitePrivacySecurityPortalSection } from '../sections/classes/site-privacy-security-checklist-portal-section.class';
import { TransactionsPortalSection } from '../sections/classes/transactions-portal-section.class';
import { UserAccessAgreementPortalSection } from '../sections/classes/user-access-agreement-portal-section.class';
import { ProfileStatus } from '../sections/models/profile-status.model';

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
      profile: this.getProfileIdentitySections(profileStatus),
      access: this.getAccessSystemsSections(profileStatus),
      training: this.getTrainingSections(profileStatus),
      documents: this.getDocumentsSections(),
    };
  }

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
        // TODO remove demo permissions when API exists
        this.permissionsService.hasRole([
          Role.FEATURE_PIDP_DEMO,
          Role.FEATURE_AMH_DEMO,
        ]) || this.insertSection('userAccessAgreement', profileStatus),
        () => [new UserAccessAgreementPortalSection(profileStatus, this.router)]
      ),
    ];
  }

  private getAccessSystemsSections(
    profileStatus: ProfileStatus
  ): IPortalSection[] {
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
    const statusCode = profileStatus.status[portalSectionKey]?.statusCode;
    return statusCode && statusCode !== StatusCode.HIDDEN;
  }
}
