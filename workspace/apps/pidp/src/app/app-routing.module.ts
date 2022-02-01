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
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes, {
      scrollPositionRestoration: 'enabled',
      onSameUrlNavigation: 'reload',
      // Does not work as expected with Material SideNav
      // being the scrollable content container
      // anchorScrolling: 'enabled',
      enableTracing: false,
    }),
  ],
  exports: [RouterModule],
})
export class AppRoutingModule {}
