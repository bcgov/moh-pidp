export class OrganizationInfoRoutes {
  public static MODULE_PATH = 'organization-info';

  public static ADMINISTRATOR_INFO = 'administrator-info';
  public static ORGANIZATION_DETAILS = 'organization-details';
  public static FACILITY_DETAILS = 'facility-details';
  public static ENDORSEMENTS = 'endorsements';

  /**
   * @description
   * Useful for redirecting to module root-level routes.
   */
  public static routePath(route: string): string {
    return `/${OrganizationInfoRoutes.MODULE_PATH}/${route}`;
  }
}
