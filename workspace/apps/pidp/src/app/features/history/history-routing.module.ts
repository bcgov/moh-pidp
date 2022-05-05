import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { HistoryRoutes } from './history.routes';
import { SignedOrAcceptedDocumentsModule } from './pages/signed-or-accepted-documents/signed-or-accepted-documents.module';
import { TransactionsModule } from './pages/transactions/transactions.module';
import { ViewDocumentModule } from './pages/view-document/view-document.module';

const routes: Routes = [
  {
    path: HistoryRoutes.TRANSACTIONS,
    loadChildren: (): Promise<TransactionsModule> =>
      import('./pages/transactions/transactions.module').then(
        (m) => m.TransactionsModule
      ),
  },
  {
    path: HistoryRoutes.SIGNED_ACCEPTED_DOCUMENTS,
    loadChildren: (): Promise<SignedOrAcceptedDocumentsModule> =>
      import(
        './pages/signed-or-accepted-documents/signed-or-accepted-documents.module'
      ).then((m) => m.SignedOrAcceptedDocumentsModule),
  },
  {
    path: `${HistoryRoutes.VIEW_DOCUMENT}/:doctype`,
    loadChildren: (): Promise<ViewDocumentModule> =>
      import('./pages/view-document/view-document.module').then(
        (m) => m.ViewDocumentModule
      ),
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class HistoryRoutingModule {}
