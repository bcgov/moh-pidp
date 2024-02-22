export class SitePrivacySecurityChecklistRoutes {
  public static BASE_PATH = 'site-privacy-and-security-checklist';

  /**
   * @description
   * Useful for redirecting to module root-level routes.
   */
  public static routePath(route: string): string {
    return `/${SitePrivacySecurityChecklistRoutes.BASE_PATH}/${route}`;
  }
}
