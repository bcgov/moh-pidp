export class PortalRoutes {
  public static BASE_PATH = 'portal';
  public static ACCESS_REQUEST_PAGE = 'accessrequest';

  /**
   * @description
   * Useful for redirecting to module root-level routes.
   */
  public static routePath(route: string): string {
    return `/${PortalRoutes.BASE_PATH}/${route}`;
  }
}
