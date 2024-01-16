import { NgModule } from '@angular/core';

import { SharedModule } from '@app/shared/shared.module';

import { ViewDocumentRoutingModule } from './view-document-routing.module';
import { ViewDocumentPage } from './view-document.page';
import { ViewDocumentDirective } from './view-document.directive';

@NgModule({
    imports: [ViewDocumentRoutingModule, SharedModule, ViewDocumentPage, ViewDocumentDirective],
})
export class ViewDocumentModule {}
