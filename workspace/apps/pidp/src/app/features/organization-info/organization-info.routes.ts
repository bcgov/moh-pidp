export class OrganizationInfoRoutes {
  public static BASE_PATH = 'organization-info';

  public static ORGANIZATION_DETAILS = 'organization-details';
  public static ENDORSEMENTS = 'endorsements';

  /**
   * @description
   * Useful for redirecting to module root-level routes.
   */
  public static routePath(route: string): string {
    return `/${OrganizationInfoRoutes.BASE_PATH}/${route}`;
  }
}
