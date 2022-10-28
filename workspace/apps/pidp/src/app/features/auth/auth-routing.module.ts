import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { AuthRoutes } from './auth.routes';
import { AuthorizationRedirectGuard } from './guards/authorization-redirect.guard';
import { LoginPageV2Component } from './pages/login-v2/login.page.component';
import { LoginPage } from './pages/login/login.page';

const routes: Routes = [
  {
    path: AuthRoutes.PORTAL_LOGIN,
    canActivate: [AuthorizationRedirectGuard],
    component: LoginPage,
    data: {
      loginPageData: {
        title: 'Provider Identity Portal',
        isAdminLogin: false,
      },
    },
  },
  {
    path: 'loginv2',
    canActivate: [AuthorizationRedirectGuard],
    component: LoginPageV2Component,
    data: {
      loginPageData: {
        title: 'Provider Identity Portal',
        isAdminLogin: false,
      },
    },
  },
  {
    path: AuthRoutes.ADMIN_LOGIN,
    canActivate: [AuthorizationRedirectGuard],
    component: LoginPage,
    data: {
      loginPageData: {
        title: 'Provider Identity Portal',
        isAdminLogin: true,
      },
      routes: {
        auth: AuthRoutes.routePath(AuthRoutes.ADMIN_LOGIN),
      },
    },
  },
  {
    path: 'adminv2',
    canActivate: [AuthorizationRedirectGuard],
    component: LoginPageV2Component,
    data: {
      loginPageData: {
        title: 'Provider Identity Portal',
        isAdminLogin: true,
      },
      routes: {
        auth: AuthRoutes.routePath(AuthRoutes.ADMIN_LOGIN),
      },
    },
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
