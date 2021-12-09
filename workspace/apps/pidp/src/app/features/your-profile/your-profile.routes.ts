export class YourProfileRoutes {
  public static MODULE_PATH = 'your-profile';

  public static TRANSACTIONS_PAGE = 'transactions';
  public static SIGNED_ACCEPTED_DOCUMENTS_PAGE = 'signed-or-accepted-documents';

  /**
   * @description
   * Useful for redirecting to module root-level routes.
   */
  public static routePath(route: string): string {
    return `/${YourProfileRoutes.MODULE_PATH}/${route}`;
  }
}
