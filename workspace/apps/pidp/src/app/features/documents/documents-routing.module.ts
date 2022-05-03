import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { PermissionsGuard } from '@app/modules/permissions/permissions.guard';
import { Role } from '@app/shared/enums/roles.enum';

import { DocumentsRoutes } from './documents.routes';
import { SignedOrAcceptedDocumentsModule } from './pages/signed-or-accepted-documents/signed-or-accepted-documents.module';
import { TransactionsModule } from './pages/transactions/transactions.module';
import { ViewDocumentModule } from './pages/view-document/view-document.module';

const routes: Routes = [
  {
    path: DocumentsRoutes.TRANSACTIONS,
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
    path: DocumentsRoutes.SIGNED_ACCEPTED_DOCUMENTS,
    loadChildren: (): Promise<SignedOrAcceptedDocumentsModule> =>
      import(
        './pages/signed-or-accepted-documents/signed-or-accepted-documents.module'
      ).then((m) => m.SignedOrAcceptedDocumentsModule),
  },
  {
    path: `${DocumentsRoutes.VIEW_DOCUMENT}/:doctype`,
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
export class DocumentsRoutingModule {}
