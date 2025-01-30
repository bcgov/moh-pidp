export class AccountLinkingRoutes {
  public static BASE_PATH = 'account-linking';

  public static EXTERNAL_GUEST_INVITATION = 'external-guest-invitation';

  /**
   * @description
   * Useful for redirecting to module root-level routes.
   */
  public static routePath(route: string): string {
    return `/${AccountLinkingRoutes.BASE_PATH}/${route}`;
  }
}
