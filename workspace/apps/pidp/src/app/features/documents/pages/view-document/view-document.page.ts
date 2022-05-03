import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import {
  DocumentService,
  IDocument,
} from '@app/core/services/document.service';

import { DocumentsRoutes } from '../../documents.routes';

@Component({
  selector: 'app-view-document',
  templateUrl: './view-document.page.html',
  styleUrls: ['./view-document.page.scss'],
})
export class ViewDocumentPage {
  public title: string;
  public document: IDocument;

  public constructor(
    private router: Router,
    route: ActivatedRoute,
    documentService: DocumentService
  ) {
    this.title = route.snapshot.data.title;
    this.document = documentService.getDocumentByType(
      route.snapshot.params.doctype
    );
  }

  public onBack(): void {
    this.router.navigate([
      DocumentsRoutes.MODULE_PATH,
      DocumentsRoutes.SIGNED_ACCEPTED_DOCUMENTS,
    ]);
  }
}
