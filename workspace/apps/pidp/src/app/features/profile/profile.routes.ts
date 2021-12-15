export class ProfileRoutes {
  public static MODULE_PATH = 'profile';

  public static PERSONAL_INFO_PAGE = 'personal-information';
  public static COLLEGE_LICENCE_INFO_PAGE = 'college-licence-information';
  public static WORK_AND_ROLE_INFO_PAGE = 'work-and-role-information';
  public static TERMS_OF_ACCESS_AGREEMENT_PAGE = 'terms-of-access-agreement';

  /**
   * @description
   * Useful for redirecting to module root-level routes.
   */
  public static routePath(route: string): string {
    return `/${ProfileRoutes.MODULE_PATH}/${route}`;
  }
}
