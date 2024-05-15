import { Routes } from '@angular/router';

import { PermissionsGuard } from '@app/modules/permissions/permissions.guard';
import { Role } from '@app/shared/enums/roles.enum';

import { AccessRoutes } from './access.routes';

export const routes: Routes = [
  {
    path: AccessRoutes.SPECIAL_AUTH_EFORMS,
    loadChildren: (): Promise<Routes> =>
      import('./pages/sa-eforms/sa-eforms-routing.routes').then(
        (m) => m.routes,
      ),
  },
  {
    path: AccessRoutes.PRESCRIPTION_REFILL_EFORMS,
    loadChildren: (): Promise<Routes> =>
      import(
        './pages/prescription-refill-eforms/prescription-refill-eforms-routing.routes'
      ).then((m) => m.routes),
  },
  {
    path: AccessRoutes.BC_PROVIDER_APPLICATION,
    loadChildren: (): Promise<Routes> =>
      import(
        './pages/bc-provider-application/bc-provider-application.route'
      ).then((m) => m.routes),
  },
  {
    path: AccessRoutes.BC_PROVIDER_EDIT,
    loadChildren: (): Promise<Routes> =>
      import('./pages/bc-provider-edit/bc-provider-edit.routes').then(
        (m) => m.routes,
      ),
  },
  {
    path: AccessRoutes.HCIM_ACCOUNT_TRANSFER,
    loadChildren: (): Promise<Routes> =>
      import(
        './pages/hcim-account-transfer/hcim-account-transfer-routing.routes'
      ).then((m) => m.routes),
  },
  {
    path: AccessRoutes.DRIVER_FITNESS,
    loadChildren: (): Promise<Routes> =>
      import('./pages/driver-fitness/driver-fitness-routing.routes').then(
        (m) => m.routes,
      ),
  },
  {
    path: AccessRoutes.MS_TEAMS_PRIVACY_OFFICER,
    loadChildren: (): Promise<Routes> =>
      import(
        './pages/ms-teams-privacy-officer/ms-teams-privacy-officer-routing.routes'
      ).then((m) => m.routes),
  },
  {
    path: AccessRoutes.PROVIDER_REPORTING_PORTAL,
    loadChildren: (): Promise<Routes> =>
      import(
        './pages/provider-reporting-portal/provider-reporting-portal-routing.routes'
      ).then((m) => m.routes),
  },
  {
    path: AccessRoutes.MS_TEAMS_CLINIC_MEMBER,
    loadChildren: (): Promise<Routes> =>
      import(
        './pages/ms-teams-clinic-member/ms-teams-clinic-member-routing.routes'
      ).then((m) => m.routes),
  },
  {
    path: AccessRoutes.IMMSBC_EFORMS,
    loadChildren: (): Promise<Routes> =>
      import('./pages/immsbc-eforms/immsbc-eforms-routing.routes').then(
        (m) => m.routes,
      ),
  },
  {
    path: AccessRoutes.EDRD_EFORMS,
    canActivate: [PermissionsGuard.canActivate],
    data: {
      roles: [Role.FEATURE_PIDP_DEMO],
    },
    loadChildren: (): Promise<Routes> =>
      import('./pages/edrd-eforms/edrd-eforms-routing.routes').then(
        (m) => m.routes,
      ),
  },
];
