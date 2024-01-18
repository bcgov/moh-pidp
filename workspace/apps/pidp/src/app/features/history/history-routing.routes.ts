import { Routes } from '@angular/router';

import { HistoryRoutes } from './history.routes';

export const routes: Routes = [
  {
    path: HistoryRoutes.TRANSACTIONS,
    loadChildren: (): Promise<Routes> =>
      import('./pages/transactions/transactions-routing.routes').then(
        (m) => m.routes,
      ),
  },
  {
    path: HistoryRoutes.SIGNED_ACCEPTED_DOCUMENTS,
    loadChildren: (): Promise<Routes> =>
      import(
        './pages/signed-or-accepted-documents/signed-or-accepted-documents-routing.routes'
      ).then((m) => m.routes),
  },
  {
    path: `${HistoryRoutes.VIEW_DOCUMENT}/:doctype`,
    loadChildren: (): Promise<Routes> =>
      import('./pages/view-document/view-document-routing.routes').then(
        (m) => m.routes,
      ),
  },
];
