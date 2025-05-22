import { Routes } from '@angular/router';

export class ProfileRoutes {
  public static readonly BASE_PATH = 'profile';

  public static readonly PERSONAL_INFO = 'personal-information';
  public static readonly COLLEGE_LICENCE_DECLARATION = 'college-licence-declaration';
  public static readonly COLLEGE_LICENCE_INFO = 'college-licence-info';
  public static readonly USER_ACCESS_AGREEMENT = 'user-access-agreement';
  public static readonly ACCOUNT_LINKING = 'account-linking';

  /**
   * @description
   * Useful for redirecting to module root-level routes.
   */
  public static routePath(route: string): string {
    return `/${ProfileRoutes.BASE_PATH}/${route}`;
  }
}

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
  {
    path: ProfileRoutes.ACCOUNT_LINKING,
    loadChildren: (): Promise<Routes> =>
      import('./pages/account-linking/account-linking-routing.routes').then(
        (m) => m.routes,
      ),
  },
];
