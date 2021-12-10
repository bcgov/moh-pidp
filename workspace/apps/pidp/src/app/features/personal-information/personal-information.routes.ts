export class PersonalInformationRoutes {
  public static MODULE_PATH = 'personal-information';

  /**
   * @description
   * Useful for redirecting to module root-level routes.
   */
  public static routePath(route: string): string {
    return `/${PersonalInformationRoutes.MODULE_PATH}/${route}`;
  }
}
