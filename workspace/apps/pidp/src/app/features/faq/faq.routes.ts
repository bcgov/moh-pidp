export class FaqRoutes {
  public static BASE_PATH = 'help';

  public static MFA_SETUP = 'mfa-setup';

  /**
   * @description
   * Useful for redirecting to module root-level routes.
   */
  public static routePath(route: string): string {
    return `/${FaqRoutes.BASE_PATH}/${route}`;
  }
}
