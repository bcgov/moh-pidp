export class AdminRoutes {
  public static BASE_PATH = 'admin';

  public static PARTIES = 'parties';

  /**
   * @description
   * Useful for redirecting to module root-level routes.
   */
  public static routePath(route: string): string {
    return `/${AdminRoutes.BASE_PATH}/${route}`;
  }
}
