import { Location } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { ArrayUtils, RouteUtils } from '@bcgov/shared/utils';

import { FeatureFlagService } from '@app/modules/feature-flag/feature-flag.service';
import { Role } from '@app/shared/enums/roles.enum';

import { YourProfileRoutes } from '../../your-profile.routes';

export interface DocumentSection {
  icon: string;
  type: string;
  title: string;
  actionLabel?: string;
  disabled?: boolean;
}

@Component({
  selector: 'app-signed-or-accepted-documents',
  templateUrl: './signed-or-accepted-documents.page.html',
  styleUrls: ['./signed-or-accepted-documents.page.scss'],
})
export class SignedOrAcceptedDocumentsPage implements OnInit {
  public title: string;
  public documents: DocumentSection[];
  private routeUtils: RouteUtils;

  public constructor(
    private featureFlagService: FeatureFlagService,
    route: ActivatedRoute,
    router: Router,
    location: Location
  ) {
    this.title = route.snapshot.data.title;
    this.documents = [];
    // TODO move into provider for each module and DI into components to reduce redundant initialization
    this.routeUtils = new RouteUtils(
      route,
      router,
      YourProfileRoutes.MODULE_PATH,
      location
    );
  }

  public onViewDocument(): void {
    this.routeUtils.routeWithin([YourProfileRoutes.VIEW_DOCUMENT_PAGE]);
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
      ...ArrayUtils.insertIf<DocumentSection>(
        this.featureFlagService.hasFlags(Role.FEATURE_PIDP_DEMO),
        [
          {
            icon: 'fingerprint',
            type: 'user-access-agreement',
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
