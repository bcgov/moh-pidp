import { NgModule } from '@angular/core';

import { SharedModule } from '@app/shared/shared.module';

import { ViewDocumentRoutingModule } from './view-document-routing.module';
import { ViewDocumentPage } from './view-document.page';
import { ViewDocumentDirective } from './view-document.directive';

@NgModule({
  declarations: [ViewDocumentPage, ViewDocumentDirective],
  imports: [ViewDocumentRoutingModule, SharedModule],
})
export class ViewDocumentModule {}
