import { NgModule } from '@angular/core';

import { SharedModule } from '@shared/shared.module';

import { SignedOrAcceptedDocumentsRoutingModule } from './signed-or-accepted-documents-routing.module';
import { SignedOrAcceptedDocumentsPage } from './signed-or-accepted-documents.page';

@NgModule({
  imports: [
    SignedOrAcceptedDocumentsRoutingModule,
    SharedModule,
    SignedOrAcceptedDocumentsPage,
  ],
})
export class SignedOrAcceptedDocumentsModule {}
