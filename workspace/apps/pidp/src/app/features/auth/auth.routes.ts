export class AuthRoutes {
  public static BASE_PATH = 'auth';

  public static ADMIN_LOGIN = 'admin';
  public static AUTO_LOGIN = 'auto-login';
  public static BC_PROVIDER_UPLIFT = 'bc-provider-uplift';
  public static PORTAL_LOGIN = 'login';

  /**
   * @description
   * Useful for redirecting to module root-level routes.
   */
  public static routePath(route: string): string {
    return `/${AuthRoutes.BASE_PATH}/${route}`;
  }
}
