import { NgModule } from '@angular/core';

import { SharedModule } from '@app/shared/shared.module';

import { ViewDocumentRoutingModule } from './view-document-routing.module';
import { ViewDocumentPage } from './view-document.page';

@NgModule({
  declarations: [ViewDocumentPage],
  imports: [ViewDocumentRoutingModule, SharedModule],
})
export class ViewDocumentModule {}
