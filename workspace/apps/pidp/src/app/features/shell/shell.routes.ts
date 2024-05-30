export class ShellRoutes {
  public static BASE_PATH = '';
  public static SUPPORT_ERROR_PAGE = 'support';
  public static ACCOUNT_LINKING_MOCK_PAGE = 'account-linking-mock'

  /**
   * @description
   * Useful for redirecting to module root-level routes.
   */
  public static routePath(route: string): string {
    return `/${ShellRoutes.BASE_PATH}/${route}`;
  }
}
