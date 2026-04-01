import { Routes } from '@angular/router';

import { AccessRoutes } from './features/access/access.routes';
import { ShellRoutes } from './features/shell/shell.routes';

export const routes: Routes = [
  {
    path: ShellRoutes.BASE_PATH,
    loadChildren: (): Promise<Routes> =>
      import('./features/shell/shell-routing.routes').then((m) => m.routes),
  },
  {
    path: AccessRoutes.BASE_PATH,
    loadChildren: (): Promise<Routes> =>
      import('./features/access/access-routing.routes').then((m) => m.routes),
  },
  {
    path: '',
    loadChildren: (): Promise<Routes> =>
      import('@bcgov/shared/root-route').then((m) => m.rootRoutes),
  },
];
