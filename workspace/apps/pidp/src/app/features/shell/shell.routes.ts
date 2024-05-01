export class ShellRoutes {
  public static BASE_PATH = '';
  public static SUPPORT_ERROR_PAGE = 'support';
  public static ACCESS_REQUEST_PAGE = 'accessrequest';

  /**
   * @description
   * Useful for redirecting to module root-level routes.
   */
  public static routePath(route: string): string {
    return `/${ShellRoutes.BASE_PATH}/${route}`;
  }
}
