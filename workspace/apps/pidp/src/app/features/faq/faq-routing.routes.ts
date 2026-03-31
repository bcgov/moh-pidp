import { Routes } from '@angular/router';

import { FaqRoutes } from './faq.routes';

export const routes: Routes = [
  {
    path: '',
    loadChildren: (): Promise<Routes> =>
      import('./pages/help/help-routing.routes').then(
        (m) => m.routes,
      ),
  },
  {
    path:FaqRoutes.MFA_SETUP,
    loadChildren:(): Promise<Routes> =>
      import('./pages/mfa-setup/mfa-setup-routing.routes').then(
        (m) => m.routes,
      ),
  },
];
