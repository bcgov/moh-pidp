import { Location } from '@angular/common';
import { Component } from '@angular/core';
import { SafeHtml } from '@angular/platform-browser';
import { ActivatedRoute, Router } from '@angular/router';

import { RouteUtils } from '@bcgov/shared/utils';

import { DocumentService } from '@app/core/services/document.service';

import { YourProfileRoutes } from '../../your-profile.routes';

@Component({
  selector: 'app-view-document',
  templateUrl: './view-document.component.html',
  styleUrls: ['./view-document.component.scss'],
})
export class ViewDocumentComponent {
  public title: string;
  public collectionNotice: SafeHtml;
  private routeUtils: RouteUtils;

  public constructor(
    route: ActivatedRoute,
    router: Router,
    location: Location,
    documentService: DocumentService
  ) {
    this.title = route.snapshot.data.title;
    this.collectionNotice = documentService.getCollectionNotice();
    // TODO move into provider for each module and DI into components to reduce redundant initialization
    this.routeUtils = new RouteUtils(
      route,
      router,
      YourProfileRoutes.MODULE_PATH,
      location
    );
  }

  public onBack(): void {
    this.routeUtils.routeWithin([
      YourProfileRoutes.SIGNED_ACCEPTED_DOCUMENTS_PAGE,
    ]);
  }
}
