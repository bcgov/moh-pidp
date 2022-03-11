import { Location } from '@angular/common';
import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { RouteUtils } from '@bcgov/shared/utils';

import {
  DocumentService,
  IDocument,
} from '@app/core/services/document.service';

import { YourDocumentsRoutes } from '../../your-documents.routes';

@Component({
  selector: 'app-view-document',
  templateUrl: './view-document.page.html',
  styleUrls: ['./view-document.page.scss'],
})
export class ViewDocumentPage {
  public title: string;
  public document: IDocument;
  private routeUtils: RouteUtils;

  public constructor(
    route: ActivatedRoute,
    router: Router,
    location: Location,
    documentService: DocumentService
  ) {
    this.title = route.snapshot.data.title;
    this.document = documentService.getDocumentByType(
      route.snapshot.params.doctype
    );
    // TODO move into provider for each module and DI into components to reduce redundant initialization
    this.routeUtils = new RouteUtils(
      route,
      router,
      YourDocumentsRoutes.MODULE_PATH,
      location
    );
  }

  public onBack(): void {
    this.routeUtils.routeWithin([
      YourDocumentsRoutes.SIGNED_ACCEPTED_DOCUMENTS_PAGE,
    ]);
  }
}
