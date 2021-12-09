import { NgModule } from '@angular/core';

import { SharedModule } from '../../shared/shared.module';

import { SignedOrAcceptedDocumentsComponent } from './pages/signed-or-accepted-documents/signed-or-accepted-documents.component';
import { TransactionsComponent } from './pages/transactions/transactions.component';
import { YourProfileRoutingModule } from './your-profile-routing.module';

@NgModule({
  declarations: [SignedOrAcceptedDocumentsComponent, TransactionsComponent],
  imports: [YourProfileRoutingModule, SharedModule],
})
export class YourProfileModule {}
