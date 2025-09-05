import { NgIf } from '@angular/common';
import { Component, OnInit, Type, ViewChild } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { ActivatedRoute, Router } from '@angular/router';

import {
  PageComponent,
  PageFooterActionDirective,
  PageFooterComponent,
  PageHeaderComponent,
  PageSectionComponent,
  SafePipe,
} from '@bcgov/shared/ui';

import {
  DocumentService,
  IDocument,
} from '@app/core/services/document.service';

import { HistoryRoutes } from '../../history.routes';
import { ViewDocumentDirective } from './view-document.directive';

@Component({
    selector: 'app-view-document',
    templateUrl: './view-document.page.html',
    styleUrls: ['./view-document.page.scss'],
    imports: [
        MatButtonModule,
        NgIf,
        PageComponent,
        PageFooterActionDirective,
        PageFooterComponent,
        PageHeaderComponent,
        PageSectionComponent,
        SafePipe,
        ViewDocumentDirective,
    ]
})
export class ViewDocumentPage implements OnInit {
  public title: string;
  public document?: IDocument;

  @ViewChild(ViewDocumentDirective, { static: true })
  public loadedDocument!: ViewDocumentDirective;

  public constructor(
    private readonly router: Router,
    private readonly route: ActivatedRoute,
    private readonly documentService: DocumentService,
  ) {
    this.title = route.snapshot.data.title;
  }

  public onBack(): void {
    this.router.navigate([
      HistoryRoutes.BASE_PATH,
      HistoryRoutes.SIGNED_ACCEPTED_DOCUMENTS,
    ]);
  }

  public ngOnInit(): void {
    const document = this.documentService.getDocumentByType(
      this.route.snapshot.params.doctype,
    );

    if (typeof document.content !== 'string') {
      this.loadComponent(document.content);
      return;
    }

    this.document = document;
  }

  private loadComponent(componentType: Type<unknown>): void {
    const viewContainerRef = this.loadedDocument.viewContainerRef;
    viewContainerRef.clear();
    viewContainerRef.createComponent<unknown>(componentType);
  }
}
