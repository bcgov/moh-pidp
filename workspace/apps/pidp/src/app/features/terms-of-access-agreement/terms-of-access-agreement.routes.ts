export class TermsOfAccessAgreementRoutes {
  public static MODULE_PATH = 'terms-of-access-agreement';

  /**
   * @description
   * Useful for redirecting to module root-level routes.
   */
  public static routePath(route: string): string {
    return `/${TermsOfAccessAgreementRoutes.MODULE_PATH}/${route}`;
  }
}
