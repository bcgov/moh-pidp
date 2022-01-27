import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { AuthRoutes } from './auth.routes';
import { IdentityProviderEnum } from './enums/identity-provider.enum';
import { AuthorizationRedirectGuard } from './guards/authorization-redirect.guard';
import { LoginComponent } from './pages/login/login.component';

const routes: Routes = [
  {
    path: AuthRoutes.PORTAL_LOGIN,
    canActivate: [AuthorizationRedirectGuard],
    component: LoginComponent,
    data: {
      title: 'Provider Identity Portal',
      idpHint: IdentityProviderEnum.BCSC,
    },
  },
  {
    path: AuthRoutes.ADMIN_LOGIN,
    canActivate: [AuthorizationRedirectGuard],
    component: LoginComponent,
    data: {
      title: 'Provider Identity Portal',
      idpHint: IdentityProviderEnum.IDIR,
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
