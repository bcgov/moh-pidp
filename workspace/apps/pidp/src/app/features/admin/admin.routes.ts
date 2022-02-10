export class AdminRoutes {
  public static MODULE_PATH = 'admin';

  public static PARTIES = 'parties';

  /**
   * @description
   * Useful for redirecting to module root-level routes.
   */
  public static routePath(route: string): string {
    return `/${AdminRoutes.MODULE_PATH}/${route}`;
  }
}
