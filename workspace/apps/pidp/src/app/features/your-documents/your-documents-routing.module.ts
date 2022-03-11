import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { PermissionsGuard } from '@app/modules/permissions/permissions.guard';
import { Role } from '@app/shared/enums/roles.enum';

import { SignedOrAcceptedDocumentsModule } from './pages/signed-or-accepted-documents/signed-or-accepted-documents.module';
import { TransactionsModule } from './pages/transactions/transactions.module';
import { ViewDocumentModule } from './pages/view-document/view-document.module';
import { YourDocumentsRoutes } from './your-documents.routes';

const routes: Routes = [
  {
    path: YourDocumentsRoutes.TRANSACTIONS_PAGE,
    canLoad: [PermissionsGuard],
    data: {
      roles: [Role.FEATURE_PIDP_DEMO],
    },
    loadChildren: (): Promise<TransactionsModule> =>
      import('./pages/transactions/transactions.module').then(
        (m) => m.TransactionsModule
      ),
  },
  {
    path: YourDocumentsRoutes.SIGNED_ACCEPTED_DOCUMENTS_PAGE,
    loadChildren: (): Promise<SignedOrAcceptedDocumentsModule> =>
      import(
        './pages/signed-or-accepted-documents/signed-or-accepted-documents.module'
      ).then((m) => m.SignedOrAcceptedDocumentsModule),
  },
  {
    path: `${YourDocumentsRoutes.VIEW_DOCUMENT_PAGE}/:doctype`,
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
export class YourDocumentsRoutingModule {}
