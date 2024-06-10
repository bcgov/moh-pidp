import { Routes } from '@angular/router';

import { FaqRoutes } from './faq.routes';

export const routes: Routes = [
  {
    path: FaqRoutes.MFA_SETUP,
    loadChildren: (): Promise<Routes> =>
      import('./pages/mfa-setup/mfa-setup-routing.routes').then(
        (m) => m.routes,
      ),
  },
];
