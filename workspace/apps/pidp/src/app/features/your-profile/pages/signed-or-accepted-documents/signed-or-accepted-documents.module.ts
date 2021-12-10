import { NgModule } from '@angular/core';

import { SharedModule } from '@shared/shared.module';

import { SignedOrAcceptedDocumentsRoutingModule } from './signed-or-accepted-documents-routing.module';
import { SignedOrAcceptedDocumentsComponent } from './signed-or-accepted-documents.component';

@NgModule({
  declarations: [SignedOrAcceptedDocumentsComponent],
  imports: [SignedOrAcceptedDocumentsRoutingModule, SharedModule],
})
export class SignedOrAcceptedDocumentsModule {}
