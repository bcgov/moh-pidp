import { Injectable } from '@angular/core';

import { DateTime } from 'luxon';

import { APP_DATE_FORMAT } from '@bcgov/shared/ui';

import { PortalSection } from '@app/features/portal/models/portal-section.model';
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
            actionDisabled: false,
          };
        }

        return section;
      }
    );
  }

  private get profileIdentitySections(): PortalSection[] {
    return [
      // {
      //   icon: 'fingerprint',
      //   type: 'personal-information',
      //   heading: 'Personal Information',
      //   hint: '1 min to complete',
      //   description: 'Personal and Contact Information',
      //   actions: [{ label: 'Update', disabled: false }],
      //   route: ProfileRoutes.routePath(ProfileRoutes.PERSONAL_INFO_PAGE),
      //   statusType: 'warn',
      //   status: 'incomplete',
      // },
      // {
      //   icon: 'fingerprint',
      //   type: 'college-licence-information',
      //   heading: 'College Licence Information',
      //   hint: '1 min to complete',
      //   description: 'College Licence Information and Validation',
      //   actions: [{ label: 'Update', disabled: false }],
      //   route: ProfileRoutes.routePath(ProfileRoutes.COLLEGE_LICENCE_INFO_PAGE),
      //   statusType: 'warn',
      //   status: 'incomplete',
      // },
      // ...ArrayUtils.insertIf<PortalSection>(
      //   this.featureFlagService.hasFlags(Role.FEATURE_PIDP_DEMO),
      //   [
      //     {
      //       icon: 'fingerprint',
      //       type: 'work-and-role-information',
      //       title: 'Work and Role Information',
      //       hint: '2 min to complete',
      //       description: 'Job title and details of your work location',
      //       actions: [{ label: 'Update', disabled: false }],
      //       route: ProfileRoutes.routePath(
      //         ProfileRoutes.WORK_AND_ROLE_INFO_PAGE
      //       ),
      //       statusType: 'warn',
      //       status: 'incomplete',
      //     },
      //     {
      //       icon: 'fingerprint',
      //       type: 'user-access-agreement',
      //       title: 'User Access Agreement(s)',
      //       hint: '13 mins to complete',
      //       description:
      //         'Read and agree to the applicable User Access Agreement(s)',
      //       actions: [{ label: 'Open', disabled: false }],
      //       route: ProfileRoutes.routePath(
      //         ProfileRoutes.USER_ACCESS_AGREEMENT_PAGE
      //       ),
      //       statusType: 'warn',
      //       status: 'incomplete',
      //     },
      //   ]
      // ),
    ];
  }

  private get accessToSystemsSections(): PortalSection[] {
    return [
      // {
      //   icon: 'fingerprint',
      //   type: 'special-authority-eforms',
      //   heading: 'Special Authority eForms',
      //   description: 'PharmaCare Special Authority eForms',
      //   actions: [{ label: 'Request', disabled: true }],
      //   route: AccessRoutes.routePath(AccessRoutes.SPECIAL_AUTH_EFORMS_PAGE),
      //   statusType: 'info',
      // },
      // ...ArrayUtils.insertIf<PortalSection>(
      //   this.featureFlagService.hasFlags(Role.FEATURE_PIDP_DEMO),
      //   [
      //     {
      //       icon: 'fingerprint',
      //       type: 'pharmanet',
      //       title: 'PharmaNet',
      //       hint: '5 mins to complete',
      //       description: 'Request access to PharmaNet',
      //       actions: [{ label: 'Request', disabled: true }],
      //       route: AccessRoutes.routePath(AccessRoutes.PHARMANET_PAGE),
      //       statusType: 'warn',
      //       status: 'incomplete',
      //     },
      //     {
      //       icon: 'fingerprint',
      //       type: 'site-privacy-and-security-readiness-checklist',
      //       title: 'Site Privacy and Security Readiness Checklist',
      //       hint: '10 mins to complete',
      //       description: 'Description of what the checklist is here',
      //       actions: [{ label: 'Request', disabled: true }],
      //       route: AccessRoutes.routePath(
      //         AccessRoutes.SITE_PRIVACY_SECURITY_CHECKLIST_PAGE
      //       ),
      //       statusType: 'warn',
      //       status: 'incomplete',
      //     },
      //   ]
      // ),
    ];
  }

  private get trainingSections(): PortalSection[] {
    return [
      // ...ArrayUtils.insertIf<PortalSection>(
      //   this.featureFlagService.hasFlags(Role.FEATURE_PIDP_DEMO),
      //   [
      //     {
      //       icon: 'fingerprint',
      //       type: 'compliance-training',
      //       title: 'Compliance Training',
      //       hint: '15 mins',
      //       description: 'Description of what the video is here',
      //       actionLabel: 'Watch',
      //       route: TrainingRoutes.routePath(
      //         TrainingRoutes.COMPLIANCE_TRAINING_PAGE
      //       ),
      //       statusType: 'warn',
      //       status: 'incomplete',
      //       disabled: false,
      //     },
      //   ]
      // ),
    ];
  }

  private get yourProfileSections(): PortalSection[] {
    return [
      // ...ArrayUtils.insertIf<PortalSection>(
      //   this.featureFlagService.hasFlags(Role.FEATURE_PIDP_DEMO),
      //   [
      //     {
      //       icon: 'fingerprint',
      //       type: 'transactions',
      //       title: 'Transactions',
      //       description: 'More information on what this is here',
      //       actions: [{ label: 'View', disabled: false }],
      //       route: YourProfileRoutes.routePath(
      //         YourProfileRoutes.TRANSACTIONS_PAGE
      //       ),
      //     },
      //   ]
      // ),
      // {
      //   icon: 'fingerprint',
      //   type: 'view-signed-or-accepted-documents',
      //   heading: 'View Signed or Accepted Documents',
      //   description: 'View Agreement(s)',
      //   actions: [{ label: 'View', disabled: false }],
      //   route: YourProfileRoutes.routePath(
      //     YourProfileRoutes.SIGNED_ACCEPTED_DOCUMENTS_PAGE
      //   ),
      // },
    ];
  }
}
