import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { FaqComponent } from './faq.component';
import { FaqRoutes } from './faq.routes';
import { MfaSetupModule } from './pages/mfa-setup/mfa-setup.module';

const routes: Routes = [
  { path: '', component: FaqComponent },
  {
    path: FaqRoutes.MFA_SETUP,
    loadChildren: (): Promise<MfaSetupModule> =>
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
