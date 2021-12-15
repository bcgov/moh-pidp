import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { AlertType } from '@bcgov/shared/ui';

import { AccessRoutes } from '../access/access.routes';
import { ProfileRoutes } from '../profile/profile.routes';
import { ShellRoutes } from '../shell/shell.routes';
import { TrainingRoutes } from '../training/training.routes';
import { YourProfileRoutes } from '../your-profile/your-profile.routes';

export interface PortalSection {
  icon: string;
  type: string;
  title: string;
  description: string;
  process: 'manual' | 'automatic';
  hint?: string;
  actionLabel?: string;
  route?: string;
  statusType?: AlertType;
  status?: string;
  disabled: boolean;
}

// TODO find a clean way to type narrowing in the template
@Component({
  selector: 'app-portal',
  templateUrl: './portal.component.html',
  styleUrls: ['./portal.component.scss'],
})
export class PortalComponent implements OnInit {
  public title: string;
  public showCollectionNotice: boolean;
  public profileIdentitySections: PortalSection[];
  public trainingSections: PortalSection[];
  public accessToSystemsSections: PortalSection[];
  public yourProfileSections: PortalSection[];

  public constructor(private route: ActivatedRoute, private router: Router) {
    this.title = this.route.snapshot.data.title;
    this.showCollectionNotice = true;

    this.profileIdentitySections = [];
    this.accessToSystemsSections = [];
    this.trainingSections = [];
    this.yourProfileSections = [];
  }

  public onAction(routePath?: string): void {
    if (!routePath) {
      return;
    }

    this.router.navigate([ShellRoutes.routePath(routePath)]);
  }

  public ngOnInit(): void {
    this.getSummaries();
  }

  public fakeStatusUpdate(type: string): void {
    if (type === 'personal-information') {
      this.profileIdentitySections = this.profileIdentitySections.map(
        (section: PortalSection) => {
          switch (section.type) {
            case 'personal-information':
              return {
                ...section,
                statusType: 'success',
                status: 'completed',
              };
            case 'provider-checklist':
              return {
                ...section,
                disabled: false,
              };
            case 'college-licence-information':
              return {
                ...section,
                disabled: false,
              };
          }

          return section;
        }
      );
      this.trainingSections = this.trainingSections.map(
        (section: PortalSection) => {
          switch (section.type) {
            case 'compliance-training':
              return {
                ...section,
                disabled: false,
              };
          }

          return section;
        }
      );
    }
    this.yourProfileSections = this.yourProfileSections.map(
      (section: PortalSection) => ({
        ...section,
        disabled: false,
      })
    );
  }

  private getSummaries(): void {
    this.profileIdentitySections = [
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
        disabled: false,
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
        disabled: false,
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
        disabled: false,
      },
    ];

    this.accessToSystemsSections = [
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
        disabled: false,
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
        disabled: false,
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
    ];

    this.trainingSections = [
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
    ];

    this.yourProfileSections = [
      {
        icon: 'fingerprint',
        type: 'transactions',
        title: 'Transactions',
        description: 'More information on what this is here',
        process: 'manual',
        actionLabel: 'View',
        route: YourProfileRoutes.routePath(YourProfileRoutes.TRANSACTIONS_PAGE),
        disabled: false,
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
        disabled: false,
      },
    ];
  }
}
