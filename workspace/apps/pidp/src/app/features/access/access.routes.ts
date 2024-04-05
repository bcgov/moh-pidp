export class AccessRoutes {
  public static BASE_PATH = 'access';

  public static BC_PROVIDER_APPLICATION = 'bc-provider-application';
  public static BC_PROVIDER_EDIT = 'bc-provider-edit';
  public static DRIVER_FITNESS = 'driver-fitness';
  public static HCIM_ACCOUNT_TRANSFER = 'hcim-account-transfer';
  public static IMMSBC_EFORMS = 'immsbc-eforms';
  public static MS_TEAMS_PRIVACY_OFFICER = 'ms-teams-privacy-officer';
  public static MS_TEAMS_CLINIC_MEMBER = 'ms-teams-clinic-member';
  public static PHARMANET = 'pharmanet';
  public static PRESCRIPTION_REFILL_EFORMS = 'prescription-refill-eforms';
  public static PROVIDER_REPORTING_PORTAL = 'provider-reporting-portal';
  public static SITE_PRIVACY_SECURITY_CHECKLIST = `site-privacy-and-security-checklist`;
  public static SPECIAL_AUTH_EFORMS = 'sa-eforms';
  public static EDRD_EFORMS = 'edrd-eforms';

  /**
   * @description
   * Useful for redirecting to module root-level routes.
   */
  public static routePath(route: string): string {
    return `/${AccessRoutes.BASE_PATH}/${route}`;
  }
}
