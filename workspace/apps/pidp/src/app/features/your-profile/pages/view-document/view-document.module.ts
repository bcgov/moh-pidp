import { NgModule } from '@angular/core';

import { SharedModule } from '@app/shared/shared.module';

import { ViewDocumentRoutingModule } from './view-document-routing.module';
import { ViewDocumentComponent } from './view-document.component';

@NgModule({
  declarations: [ViewDocumentComponent],
  imports: [ViewDocumentRoutingModule, SharedModule],
})
export class ViewDocumentModule {}
