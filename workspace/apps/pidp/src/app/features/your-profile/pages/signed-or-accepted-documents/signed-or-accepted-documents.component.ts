import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { ArrayUtils } from '@bcgov/shared/utils';

import { FeatureFlagService } from '@app/modules/feature-flag/feature-flag.service';
import { Role } from '@app/shared/enums/roles.enum';

export interface DocumentSection {
  icon: string;
  type: string;
  title: string;
  actionLabel?: string;
  disabled?: boolean;
}

@Component({
  selector: 'app-signed-or-accepted-documents',
  templateUrl: './signed-or-accepted-documents.component.html',
  styleUrls: ['./signed-or-accepted-documents.component.scss'],
})
export class SignedOrAcceptedDocumentsComponent implements OnInit {
  public title: string;
  public documents: DocumentSection[];

  public constructor(
    private route: ActivatedRoute,
    private featureFlagService: FeatureFlagService
  ) {
    this.title = this.route.snapshot.data.title;
    this.documents = [];
  }

  public ngOnInit(): void {
    this.getDocuments();
  }

  private getDocuments(): void {
    this.documents = this.documents = [
      {
        icon: 'fingerprint',
        type: 'collection-notice',
        title: 'Collection Notice',
        actionLabel: 'View',
      },
      // TODO controlled through demo feature flag
      ...ArrayUtils.insertIf<DocumentSection>(
        this.featureFlagService.hasFlag(Role.FEATURE_PIDP_DEMO),
        [
          {
            icon: 'fingerprint',
            type: 'terms-of-access',
            title: 'User Access Agreement',
            actionLabel: 'View',
            disabled: true,
          },
          {
            icon: 'fingerprint',
            type: 'pharmanet-terms-of-access',
            title: 'PharmaNet Terms of Access',
            actionLabel: 'View',
            disabled: true,
          },
        ]
      ),
    ];
  }
}
