import { Routes } from '@angular/router';

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
];
