export class HistoryRoutes {
  public static readonly BASE_PATH = 'history';

  public static readonly TRANSACTIONS = 'transactions';
  public static readonly SIGNED_ACCEPTED_DOCUMENTS = 'signed-or-accepted-documents';
  public static readonly VIEW_DOCUMENT = 'view-document';

  /**
   * @description
   * Useful for redirecting to module root-level routes.
   */
  public static routePath(route: string): string {
    return `/${HistoryRoutes.BASE_PATH}/${route}`;
  }
}
