import { Routes } from '@angular/router';

import { destinationResolver } from '@app/core/party/destination.resolver';
import { partyActionsResolver } from '@app/core/party/party-actions.resolver';
import { PermissionsGuard } from '@app/modules/permissions/permissions.guard';
import { Role } from '@app/shared/enums/roles.enum';

import { AccessRoutes } from '../access/access.routes';
import { AdminRoutes } from '../admin/admin.routes';
import { AuthRoutes } from '../auth/auth.routes';
import { AuthenticationGuard } from '../auth/guards/authentication.guard';
import { wizardResolver } from '../auth/resolvers/wizard.resolver';
import { FaqRoutes } from '../faq/faq.routes';
import { HistoryRoutes } from '../history/history.routes';
import { OrganizationInfoRoutes } from '../organization-info/organization-info.routes';
import { PortalRoutes } from '../portal/portal.routes';
import { ProfileRoutes } from '../profile/profile.routes';
import { TrainingRoutes } from '../training/training.routes';
import { PortalDashboardComponent } from './components/portal-dashboard/portal-dashboard.component';
import { ShellRoutes } from './shell.routes';

export const routes: Routes = [
  {
    path: AuthRoutes.MODULE_PATH,
    loadChildren: (): Promise<Routes> =>
      import('../auth/auth-routing.routes').then((m) => m.routes),
  },
  {
    path: AdminRoutes.MODULE_PATH,
    canMatch: [AuthenticationGuard.canMatch],
    loadChildren: (): Promise<Routes> =>
      import('../admin/admin-routing.routes').then((m) => m.routes),
  },
  {
    path: ShellRoutes.SUPPORT_ERROR_PAGE,
    loadChildren: (): Promise<Routes> =>
      import('./pages/support-error/support-error-routing.routes').then(
        (m) => m.routes,
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
        loadChildren: (): Promise<Routes> =>
          import('../portal/portal-routing.routes').then((m) => m.routes),
      },
      {
        path: ProfileRoutes.MODULE_PATH,
        loadChildren: (): Promise<Routes> =>
          import('../profile/profile.routes').then((m) => m.routes),
      },
      {
        path: OrganizationInfoRoutes.MODULE_PATH,
        resolve: {
          hasCompletedWizard: wizardResolver,
        },
        loadChildren: (): Promise<Routes> =>
          import('../organization-info/organization-info-routing.routes').then(
            (m) => m.routes,
          ),
      },
      {
        path: AccessRoutes.MODULE_PATH,
        resolve: {
          hasCompletedWizard: wizardResolver,
        },
        loadChildren: (): Promise<Routes> =>
          import('../access/access-routing.routes').then((m) => m.routes),
      },
      {
        path: TrainingRoutes.MODULE_PATH,
        resolve: {
          hasCompletedWizard: wizardResolver,
        },
        canActivate: [PermissionsGuard.canActivate],
        data: {
          roles: [Role.FEATURE_PIDP_DEMO],
        },
        loadChildren: (): Promise<Routes> =>
          import('../training/training-routing.routes').then((m) => m.routes),
      },
      {
        path: HistoryRoutes.MODULE_PATH,
        resolve: {
          hasCompletedWizard: wizardResolver,
        },
        loadChildren: (): Promise<Routes> =>
          import('../history/history-routing.routes').then((m) => m.routes),
      },
      {
        path: FaqRoutes.MODULE_PATH,
        resolve: {
          hasCompletedWizard: wizardResolver,
        },
        loadChildren: (): Promise<Routes> =>
          import('../faq/faq-routing.routes').then((m) => m.routes),
      },
      {
        path: '',
        redirectTo: PortalRoutes.MODULE_PATH,
        pathMatch: 'full',
      },
    ],
  },
];
