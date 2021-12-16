import { Injectable } from '@angular/core';

import { AccessRoutes } from '@app/features/access/access.routes';
import { PortalSection } from '@app/features/portal/portal.component';
import { ProfileRoutes } from '@app/features/profile/profile.routes';
import { TrainingRoutes } from '@app/features/training/training.routes';
import { YourProfileRoutes } from '@app/features/your-profile/your-profile.routes';

@Injectable({
  providedIn: 'root',
})
export class DemoService {
  public showCollectionNotice: boolean;
  public state: Record<string, PortalSection[]>;
  public profileComplete: boolean;

  public constructor() {
    this.showCollectionNotice = true;
    this.state = {
      profileIdentitySections: this.profileIdentitySections,
      accessToSystemsSections: this.accessToSystemsSections,
      trainingSections: this.trainingSections,
      yourProfileSections: this.yourProfileSections,
    };
    this.profileComplete = false;
  }

  public get profileIdentitySections(): PortalSection[] {
    return [
      {
        icon: 'fingerprint',
        type: 'personal-information',
        title: 'Personal Information',
        process: 'manual',
        hint: '1 min to complete',
        description: 'Name, address, and contact information',
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
        description:
          'College Licence number, practitioner ID, or on behalf user',
        actionLabel: 'Update',
        route: ProfileRoutes.routePath(ProfileRoutes.COLLEGE_LICENCE_INFO_PAGE),
        statusType: 'warn',
        status: 'incomplete',
        disabled: true,
      },
      {
        icon: 'fingerprint',
        type: 'work-and-role-information',
        title: 'Work and Role Information',
        process: 'manual',
        hint: '2 min to complete',
        description: 'Job title and details of your work location',
        actionLabel: 'Update',
        route: ProfileRoutes.routePath(ProfileRoutes.WORK_AND_ROLE_INFO_PAGE),
        statusType: 'warn',
        status: 'incomplete',
        disabled: true,
      },
      {
        icon: 'fingerprint',
        type: 'terms-of-access-agreement',
        title: 'Terms of Access Agreement',
        process: 'manual',
        hint: '13 mins to complete',
        description: 'Sign and agree to these terms of access',
        actionLabel: 'Sign',
        route: ProfileRoutes.routePath(
          ProfileRoutes.TERMS_OF_ACCESS_AGREEMENT_PAGE
        ),
        statusType: 'warn',
        status: 'incomplete',
        disabled: true,
      },
    ];
  }

  public get accessToSystemsSections(): PortalSection[] {
    return [
      {
        icon: 'fingerprint',
        type: 'gis',
        title: 'GIS',
        process: 'automatic',
        hint: 'Automatic if applicable',
        description: 'Description of what GIS is here',
        actionLabel: 'Request Manually',
        route: AccessRoutes.routePath(AccessRoutes.GIS_PAGE),
        statusType: 'info',
        disabled: true,
      },
      {
        icon: 'fingerprint',
        type: 'special-authority-eforms',
        title: 'Special Authority E-Forms',
        process: 'automatic',
        hint: 'Automatic if applicable',
        description: 'Description of what SA E-Forms is here',
        actionLabel: 'Request Manually',
        route: AccessRoutes.routePath(AccessRoutes.SPECIAL_AUTH_EFORMS_PAGE),
        statusType: 'info',
        disabled: true,
      },
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
        disabled: true,
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
        disabled: true,
      },
    ];
  }

  public get trainingSections(): PortalSection[] {
    return [
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
        disabled: true,
      },
    ];
  }

  public get yourProfileSections(): PortalSection[] {
    return [
      {
        icon: 'fingerprint',
        type: 'transactions',
        title: 'Transactions',
        description: 'More information on what this is here',
        process: 'manual',
        actionLabel: 'View',
        route: YourProfileRoutes.routePath(YourProfileRoutes.TRANSACTIONS_PAGE),
        disabled: true,
      },
      {
        icon: 'fingerprint',
        type: 'view-signed-or-accepted-documents',
        title: 'View Signed or Accepted Documents',
        description: 'More information on what this is here',
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
