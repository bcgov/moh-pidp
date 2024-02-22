export class HistoryRoutes {
  public static BASE_PATH = 'history';

  public static TRANSACTIONS = 'transactions';
  public static SIGNED_ACCEPTED_DOCUMENTS = 'signed-or-accepted-documents';
  public static VIEW_DOCUMENT = 'view-document';

  /**
   * @description
   * Useful for redirecting to module root-level routes.
   */
  public static routePath(route: string): string {
    return `/${HistoryRoutes.BASE_PATH}/${route}`;
  }
}
