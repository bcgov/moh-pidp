import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { FeatureFlagGuard } from '@app/modules/feature-flag/feature-flag.guard';
import { Role } from '@app/shared/enums/roles.enum';

import { SignedOrAcceptedDocumentsModule } from './pages/signed-or-accepted-documents/signed-or-accepted-documents.module';
import { TransactionsModule } from './pages/transactions/transactions.module';
import { ViewDocumentModule } from './pages/view-document/view-document.module';
import { YourProfileRoutes } from './your-profile.routes';

const routes: Routes = [
  {
    path: YourProfileRoutes.TRANSACTIONS_PAGE,
    canLoad: [FeatureFlagGuard],
    data: {
      features: [Role.FEATURE_PIDP_DEMO],
    },
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
  {
    path: YourProfileRoutes.VIEW_DOCUMENT_PAGE,
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
export class YourProfileRoutingModule {}
