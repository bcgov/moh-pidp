export class AccessRoutes {
  public static MODULE_PATH = 'access';

  public static SPECIAL_AUTH_EFORMS = 'sa-eforms';
  public static HCIM_ACCOUNT_TRANSFER = 'hcim-account-transfer';
  public static HCIM_ENROLMENT = 'hcim-enrolment';
  public static PHARMANET = 'pharmanet';
  public static SITE_PRIVACY_SECURITY_CHECKLIST = `site-privacy-and-security-checklist`;
  public static DRIVER_FITNESS = 'driver-fitness';
  public static UCI = 'uci';

  /**
   * @description
   * Useful for redirecting to module root-level routes.
   */
  public static routePath(route: string): string {
    return `/${AccessRoutes.MODULE_PATH}/${route}`;
  }
}
