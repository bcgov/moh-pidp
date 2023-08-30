import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { SetDashboardTitleGuard } from '@pidp/presentation';

import { AuthRoutes } from './auth.routes';
import { AuthorizationRedirectGuard } from './guards/authorization-redirect.guard';
import { AutoLoginPage } from './pages/auto-login/auto-login.page';
import { BcProviderUpliftPage } from './pages/bc-provider-uplift/bc-provider-uplift.page';
import { LoginPage } from './pages/login/login.page';

const routes: Routes = [
  {
    path: AuthRoutes.PORTAL_LOGIN,
    canActivate: [AuthorizationRedirectGuard, SetDashboardTitleGuard],
    component: LoginPage,
    data: {
      loginPageData: {
        isAdminLogin: false,
      },
      setDashboardTitleGuard: {
        titleText: '',
        titleDescriptionText: '',
      },
    },
  },
  {
    path: AuthRoutes.BC_PROVIDER_UPLIFT,
    component: BcProviderUpliftPage,
  },
  {
    path: AuthRoutes.ADMIN_LOGIN,
    canActivate: [AuthorizationRedirectGuard],
    component: LoginPage,
    data: {
      loginPageData: {
        title: 'OneHealthID Service',
        isAdminLogin: true,
      },
      routes: {
        auth: AuthRoutes.routePath(AuthRoutes.ADMIN_LOGIN),
      },
    },
  },
  {
    path: AuthRoutes.AUTO_LOGIN,
    component: AutoLoginPage,
  },
  {
    path: '',
    redirectTo: AuthRoutes.PORTAL_LOGIN,
    pathMatch: 'full',
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AuthRoutingModule {}
