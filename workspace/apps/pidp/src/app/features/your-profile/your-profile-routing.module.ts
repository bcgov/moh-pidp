import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { SignedOrAcceptedDocumentsComponent } from './pages/signed-or-accepted-documents/signed-or-accepted-documents.component';
import { TransactionsComponent } from './pages/transactions/transactions.component';
import { YourProfileRoutes } from './your-profile.routes';

const routes: Routes = [
  {
    path: YourProfileRoutes.TRANSACTIONS_PAGE,
    component: TransactionsComponent,
    data: {
      title: 'Provider Identity Portal',
    },
  },
  {
    path: YourProfileRoutes.SIGNED_ACCEPTED_DOCUMENTS_PAGE,
    component: SignedOrAcceptedDocumentsComponent,
    data: {
      title: 'Provider Identity Portal',
    },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class YourProfileRoutingModule {}
