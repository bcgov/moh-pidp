export class ShellRoutes {
  public static MODULE_PATH = '';

  /**
   * @description
   * Useful for redirecting to module root-level routes.
   */
  public static routePath(route: string): string {
    return `/${ShellRoutes.MODULE_PATH}/${route}`;
  }
}
