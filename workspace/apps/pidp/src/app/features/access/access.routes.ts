export class AccessRoutes {
  public static readonly BASE_PATH = 'access';

  public static readonly ACCESS_REQUESTS = 'access-requests';
  public static readonly BC_PROVIDER_APPLICATION = 'bc-provider-application';
  public static readonly BC_PROVIDER_EDIT = 'bc-provider-edit';
  public static readonly DRIVER_FITNESS = 'driver-fitness';
  public static readonly HCIM_ACCOUNT_TRANSFER = 'hcim-account-transfer';
  public static readonly IMMSBC_EFORMS = 'immsbc-eforms';
  public static readonly MS_TEAMS_PRIVACY_OFFICER = 'ms-teams-privacy-officer';
  public static readonly MS_TEAMS_CLINIC_MEMBER = 'ms-teams-clinic-member';
  public static readonly PRESCRIPTION_REFILL_EFORMS =
    'prescription-refill-eforms';
  public static readonly PROVIDER_REPORTING_PORTAL =
    'provider-reporting-portal';
  public static readonly SPECIAL_AUTH_EFORMS = 'sa-eforms';
  public static readonly PROVINCIAL_ATTACHMENT_SYSTEM =
    'provincial-attachment-system';
  public static readonly EXTERNAL_ACCOUNTS = 'external-accounts';
  public static readonly HALO = 'halo';
  public static readonly IVF = 'ivf';

  /**
   * @description
   * Useful for redirecting to module root-level routes.
   */
  public static routePath(route: string): string {
    return `/${AccessRoutes.BASE_PATH}/${route}`;
  }
}
