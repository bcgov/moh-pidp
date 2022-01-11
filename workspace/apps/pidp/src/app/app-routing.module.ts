import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { ShellModule } from './features/shell/shell.module';
import { ShellRoutes } from './features/shell/shell.routes';

const routes: Routes = [
  {
    path: ShellRoutes.MODULE_PATH,
    loadChildren: (): Promise<ShellModule> =>
      import('./features/shell/shell.module').then((m) => m.ShellModule),
  },
  {
    path: '',
    redirectTo: ShellRoutes.MODULE_PATH,
    pathMatch: 'full',
  },
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes, {
      scrollPositionRestoration: 'top',
      enableTracing: false,
    }),
  ],
  exports: [RouterModule],
})
export class AppRoutingModule {}
