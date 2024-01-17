import { NgModule } from '@angular/core';

import { SharedModule } from '@app/shared/shared.module';

import { ViewDocumentRoutingModule } from './view-document-routing.module';
import { ViewDocumentDirective } from './view-document.directive';
import { ViewDocumentPage } from './view-document.page';

@NgModule({
  imports: [
    ViewDocumentRoutingModule,
    SharedModule,
    ViewDocumentPage,
    ViewDocumentDirective,
  ],
})
export class ViewDocumentModule {}
