export class ProfileRoutes {
  public static MODULE_PATH = 'profile';

  public static PERSONAL_INFO = 'personal-information';
  public static COLLEGE_LICENCE_DECLARATION = 'college-licence-declaration';
  public static COLLEGE_LICENCE_INFO = 'college-licence-info';
  public static WORK_AND_ROLE_INFO = 'work-and-role-information';
  public static USER_ACCESS_AGREEMENT = 'user-access-agreement';

  /**
   * @description
   * Useful for redirecting to module root-level routes.
   */
  public static routePath(route: string): string {
    return `/${ProfileRoutes.MODULE_PATH}/${route}`;
  }
}
