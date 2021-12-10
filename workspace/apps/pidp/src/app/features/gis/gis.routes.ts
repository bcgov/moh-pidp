export class GisRoutes {
  public static MODULE_PATH = 'gis';

  /**
   * @description
   * Useful for redirecting to module root-level routes.
   */
  public static routePath(route: string): string {
    return `/${GisRoutes.MODULE_PATH}/${route}`;
  }
}
