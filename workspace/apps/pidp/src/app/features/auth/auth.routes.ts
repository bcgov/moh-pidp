export class AuthRoutes {
  public static readonly BASE_PATH = 'auth';

  public static readonly ADMIN_LOGIN = 'admin';
  public static readonly AUTO_LOGIN = 'auto-login';
  public static readonly BC_PROVIDER_UPLIFT = 'bc-provider-uplift';
  public static readonly PORTAL_LOGIN = 'login';
  public static readonly LINK_ACCOUNT_ERROR = 'link-account-error';
  public static readonly LINK_ACCOUNT_CONFIRM = 'link-account-confirm';

  /**
   * @description
   * Useful for redirecting to module root-level routes.
   */
  public static routePath(route: string): string {
    return `/${AuthRoutes.BASE_PATH}/${route}`;
  }
}
