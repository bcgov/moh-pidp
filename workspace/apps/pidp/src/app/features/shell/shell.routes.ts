export class ShellRoutes {
  public static readonly BASE_PATH = '';
  public static readonly SUPPORT_ERROR_PAGE = 'support';

  /**
   * @description
   * Useful for redirecting to module root-level routes.
   */
  public static routePath(route: string): string {
    return `/${ShellRoutes.BASE_PATH}/${route}`;
  }
}
