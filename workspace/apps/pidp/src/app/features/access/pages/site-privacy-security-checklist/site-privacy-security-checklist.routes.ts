export class SitePrivacySecurityChecklistRoutes {
  public static MODULE_PATH = 'site-privacy-and-security-checklist';

  /**
   * @description
   * Useful for redirecting to module root-level routes.
   */
  public static routePath(route: string): string {
    return `/${SitePrivacySecurityChecklistRoutes.MODULE_PATH}/${route}`;
  }
}
