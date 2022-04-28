export class OrganizationInfoRoutes {
  public static MODULE_PATH = 'organization-info';

  public static ORGANIZATION_DETAILS = 'organization-details';
  public static FACILITY_DETAILS = 'facility-details';

  /**
   * @description
   * Useful for redirecting to module root-level routes.
   */
  public static routePath(route: string): string {
    return `/${OrganizationInfoRoutes.MODULE_PATH}/${route}`;
  }
}
