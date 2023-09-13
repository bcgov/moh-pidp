import { NgModule, Type } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { FaqRoutes } from './faq.routes';
import { MfaSetupModule } from './pages/mfa-setup/mfa-setup.module';

const routes: Routes = [
  {
    path: FaqRoutes.MFA_SETUP,
    loadChildren: (): Promise<Type<MfaSetupModule>> =>
      import('./pages/mfa-setup/mfa-setup.module').then(
        (m) => m.MfaSetupModule
      ),
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class FaqRoutingModule {}
