import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { AuthRoutes } from './auth.routes';
import { IdentityProvider } from './enums/identity-provider.enum';
import { AuthorizationRedirectGuard } from './guards/authorization-redirect.guard';
import { LoginPage } from './pages/login/login.page';

const routes: Routes = [
  {
    path: AuthRoutes.PORTAL_LOGIN,
    canActivate: [AuthorizationRedirectGuard],
    component: LoginPage,
    data: {
      title: 'Provider Identity Portal',
      idpHint: IdentityProvider.BCSC,
    },
  },
  {
    path: AuthRoutes.ADMIN_LOGIN,
    canActivate: [AuthorizationRedirectGuard],
    component: LoginPage,
    data: {
      title: 'Provider Identity Portal',
      idpHint: IdentityProvider.IDIR,
      // TODO don't hardcode in the redirect URL but also don't want cross module dependencies,
      //      refactor when modules become libs otherwise premature optimization
      routes: {
        auth: '/admin',
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
