export class ShellRoutes {
  public static MODULE_PATH = '';
  public static SUPPORT_ERROR_PAGE = 'support';

  /**
   * @description
   * Useful for redirecting to module root-level routes.
   */
  public static routePath(route: string): string {
    return `/${ShellRoutes.MODULE_PATH}/${route}`;
  }
}
