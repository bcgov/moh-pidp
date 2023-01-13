export class AccessRoutes {
  public static MODULE_PATH = 'access';

  public static SPECIAL_AUTH_EFORMS = 'sa-eforms';
  public static BC_PROVIDER_APPLICATION = 'bc-provider-application';
  public static HCIM_ACCOUNT_TRANSFER = 'hcim-account-transfer';
  public static HCIM_ENROLMENT = 'hcim-enrolment';
  public static PHARMANET = 'pharmanet';
  public static SITE_PRIVACY_SECURITY_CHECKLIST = `site-privacy-and-security-checklist`;
  public static DRIVER_FITNESS = 'driver-fitness';
  public static UCI = 'uci';
  public static MS_TEAMS = 'ms-teams';
  public static PRESCRIPTION_REFILL_EFORMS = 'prescription-refill-eforms';

  /**
   * @description
   * Useful for redirecting to module root-level routes.
   */
  public static routePath(route: string): string {
    return `/${AccessRoutes.MODULE_PATH}/${route}`;
  }
}
