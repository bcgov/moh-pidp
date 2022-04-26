export class AccessRoutes {
  public static MODULE_PATH = 'access';

  public static SPECIAL_AUTH_EFORMS_PAGE = 'sa-eforms';
  public static HCIM_ACCOUNT_TRANSFER_PAGE = 'hcim-account-transfer';
  public static HCIM_ENROLMENT_PAGE = 'hcim-enrolment';
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
