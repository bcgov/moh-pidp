export class AccountsRoutes {
  public static readonly BASE_PATH = 'account';

  public static readonly ACCOUNT_LINKING = 'account-linking';
  public static readonly BC_PROVIDER_APPLICATION = 'bc-provider-application';
  public static readonly BC_PROVIDER_EDIT = 'bc-provider-edit';
  public static readonly EXTERNAL_ACCOUNTS = 'external-account';

  /**
   * @description
   * Useful for redirecting to module root-level routes.
   */
  public static routePath(route: string): string {
    return `/${AccountsRoutes.BASE_PATH}/${route}`;
  }
}
