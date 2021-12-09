export class WorkAndRoleInformationRoutes {
  public static MODULE_PATH = 'work-and-role-information';

  /**
   * @description
   * Useful for redirecting to module root-level routes.
   */
  public static routePath(route: string): string {
    return `/${WorkAndRoleInformationRoutes.MODULE_PATH}/${route}`;
  }
}
