export class PharmanetRoutes {
  public static MODULE_PATH = 'pharmanet';

  public static SELF_DECLARATION = 'self-declaration';
  public static TERMS_OF_ACCESS = 'terms-of-access';
  public static NEXT_STEPS = 'next-steps';

  /**
   * @description
   * Useful for redirecting to module root-level routes.
   */
  public static routePath(route: string): string {
    return `/${PharmanetRoutes.MODULE_PATH}/${route}`;
  }
}
