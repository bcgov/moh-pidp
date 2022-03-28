export class ShellRoutes {
  public static MODULE_PATH = '';
  public static SUPPORT_ERROR = 'support-error';

  /**
   * @description
   * Useful for redirecting to module root-level routes.
   */
  public static routePath(route: string): string {
    return `/${ShellRoutes.MODULE_PATH}/${route}`;
  }
}
