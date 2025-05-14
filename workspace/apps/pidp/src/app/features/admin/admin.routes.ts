export class AdminRoutes {
  public static readonly BASE_PATH = 'admin';

  public static readonly PARTIES = 'parties';

  /**
   * @description
   * Useful for redirecting to module root-level routes.
   */
  public static routePath(route: string): string {
    return `/${AdminRoutes.BASE_PATH}/${route}`;
  }
}
