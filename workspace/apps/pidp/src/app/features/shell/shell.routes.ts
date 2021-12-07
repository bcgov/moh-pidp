export class ShellRoutes {
  public static LOGIN_PAGE = 'login';

  public static MODULE_PATH = 'pidp';

  public static PORTAL_PAGE = 'portal';

  /**
   * @description
   * Useful for redirecting to module root-level routes.
   */
  public static routePath(route: string): string {
    return `/${ShellRoutes.MODULE_PATH}/${route}`;
  }
}
