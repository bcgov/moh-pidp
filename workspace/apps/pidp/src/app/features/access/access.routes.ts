export class AccessRoutes {
  public static MODULE_PATH = 'profile';

  public static GIS_PAGE = 'gis';
  public static SPECIAL_AUTH_EFORMS_PAGE = 'special-authority-eforms';

  /**
   * @description
   * Useful for redirecting to module root-level routes.
   */
  public static routePath(route: string): string {
    return `/${AccessRoutes.MODULE_PATH}/${route}`;
  }
}
