import { Routes } from '@angular/router';

import { ShellRoutes } from './features/shell/shell.routes';

export const routes: Routes = [
  {
    path: ShellRoutes.BASE_PATH,
    loadChildren: (): Promise<Routes> =>
      import('./features/shell/shell-routing.routes').then((m) => m.routes),
  },
  {
    path: '',
    loadChildren: (): Promise<Routes> =>
      import('@bcgov/shared/root-route').then((m) => m.rootRoutes),
  },
  {
    path: ShellRoutes.ACCESS_REQUEST_PAGE,
    loadChildren: (): Promise<Routes> =>
      import('./features/access/access-routing.routes').then((m) => m.routes),
  },
];
