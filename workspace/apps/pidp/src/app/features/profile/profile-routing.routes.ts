import { Routes } from '@angular/router';

import { PermissionsGuard } from '@app/modules/permissions/permissions.guard';
import { Role } from '@app/shared/enums/roles.enum';

import { ProfileRoutes } from './profile.routes';

export const routes: Routes = [
  {
    path: ProfileRoutes.PERSONAL_INFO,
    loadChildren: (): Promise<Routes> =>
      import(
        './pages/personal-information/personal-information-routing.routes'
      ).then((m) => m.routes),
  },
  {
    path: ProfileRoutes.COLLEGE_LICENCE_DECLARATION,
    loadChildren: (): Promise<Routes> =>
      import(
        './pages/college-licence/college-licence-declaration/college-licence-declaration-routing.routes'
      ).then((m) => m.routes),
  },
  {
    path: ProfileRoutes.WORK_AND_ROLE_INFO,
    canMatch: [PermissionsGuard.canMatch],
    data: {
      roles: [Role.FEATURE_PIDP_DEMO],
    },
    loadChildren: (): Promise<Routes> =>
      import(
        './pages/work-and-role-information/work-and-role-information-routing.routes'
      ).then((m) => m.routes),
  },
  {
    path: ProfileRoutes.USER_ACCESS_AGREEMENT,
    loadChildren: (): Promise<Routes> =>
      import(
        './pages/user-access-agreement/user-access-agreement-routing.routes'
      ).then((m) => m.routes),
  },
  {
    path: ProfileRoutes.COLLEGE_LICENCE_INFO,
    loadChildren: (): Promise<Routes> =>
      import(
        './pages/college-licence/college-licence-information/college-licence-information-routing.routes'
      ).then((m) => m.routes),
  },
  {
    path: ProfileRoutes.ACCOUNT_LINKING,
    loadChildren: (): Promise<Routes> =>
      import('./pages/account-linking/account-linking-routing.routes').then(
        (m) => m.routes,
      ),
  },
];
