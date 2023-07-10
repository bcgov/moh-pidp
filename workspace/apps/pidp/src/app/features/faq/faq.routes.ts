export class FaqRoutes {
  public static MODULE_PATH = 'faq';

  public static MFA_SETUP = 'mfa-setup';

  /**
   * @description
   * Useful for redirecting to module root-level routes.
   */
  public static routePath(route: string): string {
    return `/${FaqRoutes.MODULE_PATH}/${route}`;
  }
}
