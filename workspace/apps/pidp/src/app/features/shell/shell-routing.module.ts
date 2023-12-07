import { PortalModule } from '@angular/cdk/portal';
import { NgModule, Type } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { PermissionsGuard } from '@app/modules/permissions/permissions.guard';
import { Role } from '@app/shared/enums/roles.enum';

import { AccessModule } from '../access/access.module';
import { AccessRoutes } from '../access/access.routes';
import { AdminModule } from '../admin/admin.module';
import { AdminRoutes } from '../admin/admin.routes';
import { AuthModule } from '../auth/auth.module';
import { AuthRoutes } from '../auth/auth.routes';
import { AuthenticationGuard } from '../auth/guards/authentication.guard';
import { FaqModule } from '../faq/faq.module';
import { FaqRoutes } from '../faq/faq.routes';
import { HistoryModule } from '../history/history.module';
import { HistoryRoutes } from '../history/history.routes';
import { OrganizationInfoModule } from '../organization-info/organization-info.module';
import { OrganizationInfoRoutes } from '../organization-info/organization-info.routes';
import { PortalRoutes } from '../portal/portal.routes';
import { ProfileModule } from '../profile/profile.module';
import { ProfileRoutes } from '../profile/profile.routes';
import { SupportErrorModule } from '../shell/pages/support-error/support-error.module';
import { TrainingModule } from '../training/training.module';
import { TrainingRoutes } from '../training/training.routes';
import { PortalDashboardComponent } from './components/portal-dashboard/portal-dashboard.component';
import { ShellRoutes } from './shell.routes';
import { destinationResolver } from '@app/core/party/destination.resolver';
import { partyActionsResolver } from '@app/core/party/party-actions.resolver';

const routes: Routes = [
  {
    path: AuthRoutes.MODULE_PATH,
    loadChildren: (): Promise<Type<AuthModule>> =>
      import('../auth/auth.module').then((m) => m.AuthModule),
  },
  {
    path: AdminRoutes.MODULE_PATH,
    canMatch: [AuthenticationGuard.canMatch],
    loadChildren: (): Promise<Type<AdminModule>> =>
      import('../admin/admin.module').then((m) => m.AdminModule),
  },
  {
    path: ShellRoutes.SUPPORT_ERROR_PAGE,
    loadChildren: (): Promise<Type<SupportErrorModule>> =>
      import('../shell/pages/support-error/support-error.module').then(
        (m) => m.SupportErrorModule,
      ),
  },
  {
    path: '',
    component: PortalDashboardComponent,
    canActivate: [AuthenticationGuard.canActivate],
    canActivateChild: [AuthenticationGuard.canActivateChild],
    resolve: {
      partyId: partyActionsResolver,
    },
    data: {
      routes: {
        auth: `/${AuthRoutes.MODULE_PATH}`,
      },
    },
    children: [
      {
        path: PortalRoutes.MODULE_PATH,
        resolve: {
          destination: destinationResolver,
        },
        loadChildren: (): Promise<Type<PortalModule>> =>
          import('../portal/portal.module').then((m) => m.PortalModule),
      },
      {
        path: ProfileRoutes.MODULE_PATH,
        loadChildren: (): Promise<Type<ProfileModule>> =>
          import('../profile/profile.module').then((m) => m.ProfileModule),
      },
      {
        path: OrganizationInfoRoutes.MODULE_PATH,
        loadChildren: (): Promise<Type<OrganizationInfoModule>> =>
          import('../organization-info/organization-info.module').then(
            (m) => m.OrganizationInfoModule,
          ),
      },
      {
        path: AccessRoutes.MODULE_PATH,
        loadChildren: (): Promise<Type<AccessModule>> =>
          import('../access/access.module').then((m) => m.AccessModule),
      },
      {
        path: TrainingRoutes.MODULE_PATH,
        canActivate: [PermissionsGuard.canActivate],
        data: {
          roles: [Role.FEATURE_PIDP_DEMO],
        },
        loadChildren: (): Promise<Type<TrainingModule>> =>
          import('../training/training.module').then((m) => m.TrainingModule),
      },
      {
        path: HistoryRoutes.MODULE_PATH,
        loadChildren: (): Promise<Type<HistoryModule>> =>
          import('../history/history.module').then((m) => m.HistoryModule),
      },
      {
        path: FaqRoutes.MODULE_PATH,
        loadChildren: (): Promise<Type<FaqModule>> =>
          import('../faq/faq.module').then((m) => m.FaqModule),
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
