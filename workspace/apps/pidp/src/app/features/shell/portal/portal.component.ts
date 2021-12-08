import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

export interface PortalSection {
  icon: string;
  type: string;
  title: string;
  process: 'manual' | 'automatic';
  hint?: string;
  actionLabel?: string;
  statusType?: 'info' | 'warn';
  status?: string;
}

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

  public constructor(private route: ActivatedRoute) {
    this.title = this.route.snapshot.data.title;
    this.showCollectionNotice = true;

    this.profileIdentitySections = [
      {
        icon: 'fingerprint',
        type: 'personal-information',
        title: 'Personal Information',
        process: 'manual',
        hint: '1 min to complete',
        actionLabel: 'Update',
        statusType: 'warn',
        status: 'incomplete',
      },
      {
        icon: 'fingerprint',
        type: 'provider-checklist',
        title: 'Provider Checklist',
        process: 'manual',
        hint: '12 mins to complete',
        actionLabel: 'Update',
        statusType: 'warn',
        status: 'incomplete',
      },
      {
        icon: 'fingerprint',
        type: 'college-licence-information',
        title: 'College Licence Information',
        process: 'manual',
        hint: '1 min to complete',
        actionLabel: 'Update',
        statusType: 'warn',
        status: 'incomplete',
      },
      {
        icon: 'fingerprint',
        type: 'work-and-role-information',
        title: 'Work and Role Information',
        process: 'manual',
        hint: '1 min to complete',
        actionLabel: 'Update',
        statusType: 'warn',
        status: 'incomplete',
      },
      {
        icon: 'fingerprint',
        type: 'plr',
        title: 'PLR',
        process: 'manual',
        hint: '1 min to complete',
        actionLabel: 'Update',
        statusType: 'warn',
        status: 'incomplete',
      },
      {
        icon: 'fingerprint',
        type: 'terms-of-access-agreement',
        title: 'Terms of Access Agreement',
        process: 'manual',
        hint: '13 mins to complete',
        actionLabel: 'Sign',
        statusType: 'warn',
        status: 'incomplete',
      },
    ];

    this.trainingSections = [
      {
        icon: 'fingerprint',
        type: 'compliance-training',
        title: 'Compliance Training',
        process: 'manual',
        hint: '15 mins to complete',
        actionLabel: 'Watch',
        statusType: 'warn',
        status: 'incomplete',
      },
    ];

    this.accessToSystemsSections = [
      {
        icon: 'fingerprint',
        type: 'access-to-pharmanet',
        title: 'Access to PharmaNet',
        process: 'manual',
        hint: '7 mins to complete',
        actionLabel: 'Request',
        statusType: 'warn',
        status: 'incomplete',
      },
      {
        icon: 'fingerprint',
        type: 'access-to-pharmanet-site-registration',
        title: 'Access to PharmaNet Site Registration',
        process: 'manual',
        hint: '12 mins to complete',
        actionLabel: 'Request',
        statusType: 'warn',
        status: 'incomplete',
      },
      {
        icon: 'fingerprint',
        type: 'gis',
        title: 'GIS',
        process: 'automatic',
        hint: 'Automatic if applicable',
        statusType: 'info',
      },
      {
        icon: 'fingerprint',
        type: 'special-authority-eforms',
        title: 'Special Authority E-Forms',
        process: 'automatic',
        hint: 'Automatic if applicable',
        statusType: 'info',
      },
    ];

    this.yourProfileSections = [
      {
        icon: 'fingerprint',
        type: 'view-signed-or-accepted-documents',
        title: 'View Signed or Accepted Documents',
        process: 'manual',
        actionLabel: 'View',
      },
      {
        icon: 'fingerprint',
        type: 'terms-of-access-agreement',
        title: 'Terms of Access Agreement',
        process: 'manual',
        actionLabel: 'View',
      },
    ];
  }

  public ngOnInit(): void {}
}
