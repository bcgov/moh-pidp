export class SpecialAuthorityEformsRoutes {
  public static MODULE_PATH = 'special-authority-eforms';

  /**
   * @description
   * Useful for redirecting to module root-level routes.
   */
  public static routePath(route: string): string {
    return `/${SpecialAuthorityEformsRoutes.MODULE_PATH}/${route}`;
  }
}
