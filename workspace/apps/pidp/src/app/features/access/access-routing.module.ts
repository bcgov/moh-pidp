import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { AccessRoutes } from './access.routes';
import { SpecialAuthorityEformsModule } from './pages/special-authority-eforms/special-authority-eforms.module';

const routes: Routes = [
  {
    path: AccessRoutes.SPECIAL_AUTH_EFORMS_PAGE,
    loadChildren: (): Promise<SpecialAuthorityEformsModule> =>
      import(
        './pages/special-authority-eforms/special-authority-eforms-routing.module'
      ).then((m) => m.SpecialAuthorityEformsRoutingModule),
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AccessRoutingModule {}
