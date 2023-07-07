import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';



import { SetDashboardTitleGuard } from '@pidp/presentation';



import { MfaSetupPage } from './mfa-setup.page';


const routes: Routes = [
  {
    path: '',
    component: MfaSetupPage,
    canActivate: [SetDashboardTitleGuard],
    data: {
      title: 'BCProvider AD Multi-factor Authentication (MFA) Setup',
      routes: {
        root: '../../',
      },
      setDashboardTitleGuard: {
        titleText: '',
        titleDescriptionText: '',
      },
    },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class MfaSetupRoutingModule {}
