export class CollegeLicenceInformationRoutes {
  public static MODULE_PATH = 'college-licence-information';

  /**
   * @description
   * Useful for redirecting to module root-level routes.
   */
  public static routePath(route: string): string {
    return `/${CollegeLicenceInformationRoutes.MODULE_PATH}/${route}`;
  }
}
