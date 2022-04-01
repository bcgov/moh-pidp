import { PortalModule } from '@angular/cdk/portal';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { UserGuard } from '@app/core/guards/user.guard';
import { PartyResolver } from '@app/core/party/party.resolver';
import { PermissionsGuard } from '@app/modules/permissions/permissions.guard';
import { Role } from '@app/shared/enums/roles.enum';

import { AccessModule } from '../access/access.module';
import { AccessRoutes } from '../access/access.routes';
import { AdminModule } from '../admin/admin.module';
import { AdminRoutes } from '../admin/admin.routes';
import { AuthModule } from '../auth/auth.module';
import { AuthRoutes } from '../auth/auth.routes';
import { AuthenticationGuard } from '../auth/guards/authentication.guard';
import { DocumentsModule } from '../documents/documents.module';
import { DocumentsRoutes } from '../documents/documents.routes';
import { PortalRoutes } from '../portal/portal.routes';
import { ProfileModule } from '../profile/profile.module';
import { ProfileRoutes } from '../profile/profile.routes';
import { TrainingModule } from '../training/training.module';
import { TrainingRoutes } from '../training/training.routes';
import { PortalDashboardComponent } from './components/portal-dashboard/portal-dashboard.component';

const routes: Routes = [
  {
    path: AuthRoutes.MODULE_PATH,
    loadChildren: (): Promise<AuthModule> =>
      import('../auth/auth.module').then((m) => m.AuthModule),
  },
  {
    path: AdminRoutes.MODULE_PATH,
    canLoad: [AuthenticationGuard],
    loadChildren: (): Promise<AdminModule> =>
      import('../admin/admin.module').then((m) => m.AdminModule),
  },
  {
    path: '',
    component: PortalDashboardComponent,
    canActivate: [AuthenticationGuard, UserGuard],
    canActivateChild: [AuthenticationGuard],
    resolve: {
      partyId: PartyResolver,
    },
    data: {
      // TODO don't hardcode in the redirect URL but also don't want cross module dependencies,
      //      refactor when modules become libs otherwise premature optimization
      routes: {
        auth: '/auth',
      },
    },
    children: [
      {
        path: PortalRoutes.MODULE_PATH,
        loadChildren: (): Promise<PortalModule> =>
          import('../portal/portal.module').then((m) => m.PortalModule),
      },
      {
        path: ProfileRoutes.MODULE_PATH,
        loadChildren: (): Promise<ProfileModule> =>
          import('../profile/profile.module').then((m) => m.ProfileModule),
      },
      {
        path: AccessRoutes.MODULE_PATH,
        loadChildren: (): Promise<AccessModule> =>
          import('../access/access.module').then((m) => m.AccessModule),
      },
      {
        path: TrainingRoutes.MODULE_PATH,
        canActivate: [PermissionsGuard],
        data: {
          roles: [Role.FEATURE_PIDP_DEMO, Role.FEATURE_AMH_DEMO],
        },
        loadChildren: (): Promise<TrainingModule> =>
          import('../training/training.module').then((m) => m.TrainingModule),
      },
      {
        path: DocumentsRoutes.MODULE_PATH,
        loadChildren: (): Promise<DocumentsModule> =>
          import('../documents/documents.module').then(
            (m) => m.DocumentsModule
          ),
      },
      {
        path: '',
        redirectTo: PortalRoutes.MODULE_PATH,
        pathMatch: 'full',
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ShellRoutingModule {}
