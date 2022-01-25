import { Injectable } from '@angular/core';

import { DateTime } from 'luxon';

import { APP_DATE_FORMAT } from '@bcgov/shared/ui';
import { ArrayUtils } from '@bcgov/shared/utils';

import { AccessRoutes } from '@app/features/access/access.routes';
import { PortalSection } from '@app/features/portal/portal.component';
import { ProfileRoutes } from '@app/features/profile/profile.routes';
import { TrainingRoutes } from '@app/features/training/training.routes';
import { YourProfileRoutes } from '@app/features/your-profile/your-profile.routes';
import { FeatureFlagService } from '@app/modules/feature-flag/feature-flag.service';
import { Role } from '@app/shared/enums/roles.enum';

@Injectable({
  providedIn: 'root',
})
export class DemoService {
  public showCollectionNotice: boolean;
  public state: Record<string, PortalSection[]>;
  public profileComplete: boolean;

  public constructor(private featureFlagService: FeatureFlagService) {
    this.showCollectionNotice = true;
    this.state = {
      profileIdentitySections: this.profileIdentitySections,
      accessToSystemsSections: this.accessToSystemsSections,
      trainingSections: this.trainingSections,
      yourProfileSections: this.yourProfileSections,
    };
    this.profileComplete = false;
  }

  public updateState(sectionType: string): void {
    let enableMap: { [key: string]: string } = {
      'personal-information': 'college-licence-information',
    };

    if (this.featureFlagService.hasFlags(Role.FEATURE_PIDP_DEMO)) {
      enableMap = {
        ...enableMap,
        'college-licence-information': 'work-and-role-information',
        'work-and-role-information': 'user-access-agreement',
      };
    }

    this.state.profileIdentitySections = this.state.profileIdentitySections.map(
      (section) => {
        if (section.type === sectionType) {
          return {
            ...section,
            statusType: 'success',
            status: 'completed',
            hint: DateTime.now().toFormat(APP_DATE_FORMAT),
          };
        }
        if (section.type === enableMap[sectionType]) {
          return {
            ...section,
            disabled: false,
          };
        }

        return section;
      }
    );
  }

  private get profileIdentitySections(): PortalSection[] {
    return [
      {
        icon: 'fingerprint',
        type: 'personal-information',
        title: 'Personal Information',
        process: 'manual',
        hint: '1 min to complete',
        description: 'Personal and Contact Information',
        actionLabel: 'Update',
        route: ProfileRoutes.routePath(ProfileRoutes.PERSONAL_INFO_PAGE),
        statusType: 'warn',
        status: 'incomplete',
        disabled: false,
      },
      {
        icon: 'fingerprint',
        type: 'college-licence-information',
        title: 'College Licence Information',
        process: 'manual',
        hint: '1 min to complete',
        description: 'College Licence Information and Validation',
        actionLabel: 'Update',
        route: ProfileRoutes.routePath(ProfileRoutes.COLLEGE_LICENCE_INFO_PAGE),
        statusType: 'warn',
        status: 'incomplete',
        disabled: false,
      },
      ...ArrayUtils.insertIf<PortalSection>(
        this.featureFlagService.hasFlags(Role.FEATURE_PIDP_DEMO),
        [
          {
            icon: 'fingerprint',
            type: 'work-and-role-information',
            title: 'Work and Role Information',
            process: 'manual',
            hint: '2 min to complete',
            description: 'Job title and details of your work location',
            actionLabel: 'Update',
            route: ProfileRoutes.routePath(
              ProfileRoutes.WORK_AND_ROLE_INFO_PAGE
            ),
            statusType: 'warn',
            status: 'incomplete',
            disabled: false,
          },
          {
            icon: 'fingerprint',
            type: 'user-access-agreement',
            title: 'User Access Agreement(s)',
            process: 'manual',
            hint: '13 mins to complete',
            description:
              'Read and agree to the applicable User Access Agreement(s)',
            actionLabel: 'Open',
            route: ProfileRoutes.routePath(
              ProfileRoutes.USER_ACCESS_AGREEMENT_PAGE
            ),
            statusType: 'warn',
            status: 'incomplete',
            disabled: false,
          },
        ]
      ),
    ];
  }

  private get accessToSystemsSections(): PortalSection[] {
    return [
      {
        icon: 'fingerprint',
        type: 'special-authority-eforms',
        title: 'Special Authority E-Forms',
        process: 'automatic',
        description: 'PharmaCare Special Authority eForms',
        actionLabel: 'Request',
        route: AccessRoutes.routePath(AccessRoutes.SPECIAL_AUTH_EFORMS_PAGE),
        statusType: 'info',
        disabled: true,
      },
      ...ArrayUtils.insertIf<PortalSection>(
        this.featureFlagService.hasFlags(Role.FEATURE_PIDP_DEMO),
        [
          {
            icon: 'fingerprint',
            type: 'pharmanet',
            title: 'PharmaNet',
            process: 'manual',
            hint: '5 mins to complete',
            description: 'Request access to PharmaNet',
            actionLabel: 'Request',
            route: AccessRoutes.routePath(AccessRoutes.PHARMANET_PAGE),
            statusType: 'warn',
            status: 'incomplete',
            disabled: false,
          },
          {
            icon: 'fingerprint',
            type: 'site-privacy-and-security-readiness-checklist',
            title: 'Site Privacy and Security Readiness Checklist',
            process: 'manual',
            hint: '10 mins to complete',
            description: 'Description of what the checklist is here',
            actionLabel: 'Request',
            route: AccessRoutes.routePath(
              AccessRoutes.SITE_PRIVACY_SECURITY_CHECKLIST_PAGE
            ),
            statusType: 'warn',
            status: 'incomplete',
            disabled: false,
          },
        ]
      ),
    ];
  }

  private get trainingSections(): PortalSection[] {
    return [
      ...ArrayUtils.insertIf<PortalSection>(
        this.featureFlagService.hasFlags(Role.FEATURE_PIDP_DEMO),
        [
          {
            icon: 'fingerprint',
            type: 'compliance-training',
            title: 'Compliance Training',
            process: 'manual',
            hint: '15 mins',
            description: 'Description of what the video is here',
            actionLabel: 'Watch',
            route: TrainingRoutes.routePath(
              TrainingRoutes.COMPLIANCE_TRAINING_PAGE
            ),
            statusType: 'warn',
            status: 'incomplete',
            disabled: false,
          },
        ]
      ),
    ];
  }

  private get yourProfileSections(): PortalSection[] {
    return [
      ...ArrayUtils.insertIf<PortalSection>(
        this.featureFlagService.hasFlags(Role.FEATURE_PIDP_DEMO),
        [
          {
            icon: 'fingerprint',
            type: 'transactions',
            title: 'Transactions',
            description: 'More information on what this is here',
            process: 'manual',
            actionLabel: 'View',
            route: YourProfileRoutes.routePath(
              YourProfileRoutes.TRANSACTIONS_PAGE
            ),
            disabled: false,
          },
        ]
      ),
      {
        icon: 'fingerprint',
        type: 'view-signed-or-accepted-documents',
        title: 'View Signed or Accepted Documents',
        description: 'View Agreement(s)',
        process: 'manual',
        actionLabel: 'View',
        route: YourProfileRoutes.routePath(
          YourProfileRoutes.SIGNED_ACCEPTED_DOCUMENTS_PAGE
        ),
        disabled: true,
      },
    ];
  }
}
