import { Routes } from '@angular/router';

import { PortalDashboardComponent } from '../shell/components/portal-dashboard/portal-dashboard.component';
import { AuthRoutes } from './auth.routes';
import { authorizationRedirectGuard } from './guards/authorization-redirect.guard';
import { AutoLoginPage } from './pages/auto-login/auto-login.page';
import { BcProviderUpliftPage } from './pages/bc-provider-uplift/bc-provider-uplift.page';
import { LinkAccountConfirmPage } from './pages/link-account-confirm/link-account-confirm.page';
import { LinkAccountErrorPage } from './pages/link-account-error/link-account-error.page';
import { LoginPage } from './pages/login/login.page';

export const routes: Routes = [
  {
    path: AuthRoutes.PORTAL_LOGIN,
    canActivate: [authorizationRedirectGuard],
    component: LoginPage,
    data: {
      loginPageData: {
        isAdminLogin: false,
      },
    },
  },
  {
    path: AuthRoutes.BC_PROVIDER_UPLIFT,
    component: BcProviderUpliftPage,
  },
  {
    path: AuthRoutes.LINK_ACCOUNT_ERROR,
    component: LinkAccountErrorPage,
  },
  {
    path: AuthRoutes.ADMIN_LOGIN,
    canActivate: [authorizationRedirectGuard],
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
  {
    path: '',
    component: PortalDashboardComponent,
    children: [
      {
        path: AuthRoutes.LINK_ACCOUNT_CONFIRM,
        component: LinkAccountConfirmPage,
      },
    ],
  },
];
