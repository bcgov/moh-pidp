import { Location } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { RouteUtils } from '@bcgov/shared/utils';

import {
  DocumentService,
  DocumentType,
  IDocumentMetaData,
} from '@app/core/services/document.service';

import { YourProfileRoutes } from '../../your-profile.routes';

export interface DocumentSection extends IDocumentMetaData {
  icon: string;
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
    private documentService: DocumentService,
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

  public onViewDocument(documentType: DocumentType): void {
    this.routeUtils.routeWithin([
      YourProfileRoutes.VIEW_DOCUMENT_PAGE,
      documentType,
    ]);
  }

  public ngOnInit(): void {
    this.documents = this.documentService.getDocuments().map((document) => {
      return { icon: 'fingerprint', ...document, actionLabel: 'View' };
    });
  }
}
