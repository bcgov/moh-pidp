export class AccessRoutes {
  public static MODULE_PATH = 'profile';

  public static GIS_PAGE = 'gis';
  public static SPECIAL_AUTH_EFORMS_PAGE = 'special-authority-eforms';
  public static HCIM_WEB_ENROLMENT = 'hcim-web-enrolment';
  public static PHARMANET_PAGE = 'pharmanet';
  public static SITE_PRIVACY_SECURITY_CHECKLIST_PAGE =
    'site-privacy-and-security-checklist';

  /**
   * @description
   * Useful for redirecting to module root-level routes.
   */
  public static routePath(route: string): string {
    return `/${AccessRoutes.MODULE_PATH}/${route}`;
  }
}
