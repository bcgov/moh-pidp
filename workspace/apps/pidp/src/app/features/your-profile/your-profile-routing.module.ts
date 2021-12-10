import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { SignedOrAcceptedDocumentsModule } from './pages/signed-or-accepted-documents/signed-or-accepted-documents.module';
import { TransactionsModule } from './pages/transactions/transactions.module';
import { YourProfileRoutes } from './your-profile.routes';

const routes: Routes = [
  {
    path: YourProfileRoutes.TRANSACTIONS_PAGE,
    loadChildren: (): Promise<TransactionsModule> =>
      import('./pages/transactions/transactions.module').then(
        (m) => m.TransactionsModule
      ),
  },
  {
    path: YourProfileRoutes.SIGNED_ACCEPTED_DOCUMENTS_PAGE,
    loadChildren: (): Promise<SignedOrAcceptedDocumentsModule> =>
      import(
        './pages/signed-or-accepted-documents/signed-or-accepted-documents.module'
      ).then((m) => m.SignedOrAcceptedDocumentsModule),
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class YourProfileRoutingModule {}
