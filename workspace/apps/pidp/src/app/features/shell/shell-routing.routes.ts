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
    path: AuthRoutes.BASE_PATH,
    loadChildren: (): Promise<Routes> =>
      import('../auth/auth-routing.routes').then((m) => m.routes),
  },
  {
    path: AdminRoutes.BASE_PATH,
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
        auth: `/${AuthRoutes.BASE_PATH}`,
      },
    },
    children: [
      {
        path: PortalRoutes.BASE_PATH,
        resolve: {
          destination: destinationResolver,
        },
        loadChildren: (): Promise<Routes> =>
          import('../portal/portal-routing.routes').then((m) => m.routes),
      },
      {
        path: ProfileRoutes.BASE_PATH,
        loadChildren: (): Promise<Routes> =>
          import('../profile/profile.routes').then((m) => m.routes),
      },
      {
        path: AccessRoutes.BASE_PATH,
        loadChildren: (): Promise<Routes> =>
          import('../access/components/access-request-page/access-request-page-routing.routes').then(
            (m) => m.routes,
          ),
      },
      {
        path: OrganizationInfoRoutes.BASE_PATH,
        resolve: {
          hasCompletedWizard: wizardResolver,
        },
        loadChildren: (): Promise<Routes> =>
          import('../organization-info/organization-info-routing.routes').then(
            (m) => m.routes,
          ),
      },
      {
        path: TrainingRoutes.BASE_PATH,
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
        path: HistoryRoutes.BASE_PATH,
        resolve: {
          hasCompletedWizard: wizardResolver,
        },
        loadChildren: (): Promise<Routes> =>
          import('../history/history-routing.routes').then((m) => m.routes),
      },
      {
        path: FaqRoutes.BASE_PATH,
        resolve: {
          hasCompletedWizard: wizardResolver,
        },
        loadChildren: (): Promise<Routes> =>
          import('../faq/faq-routing.routes').then((m) => m.routes),
      },
      {
        path: '',
        redirectTo: PortalRoutes.BASE_PATH,
        pathMatch: 'full',
      },
    ],
  },
];
