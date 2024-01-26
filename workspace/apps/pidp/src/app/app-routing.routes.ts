import { Routes } from '@angular/router';

import { ShellRoutes } from './features/shell/shell.routes';

export const routes: Routes = [
  {
    path: ShellRoutes.MODULE_PATH,
    loadChildren: (): Promise<Routes> =>
      import('./features/shell/shell-routing.routes').then((m) => m.routes),
  },
  {
    path: '',
    loadChildren: (): Promise<Routes> =>
      import('@bcgov/shared/ui').then((m) => m.rootRoutes),
  },
];
