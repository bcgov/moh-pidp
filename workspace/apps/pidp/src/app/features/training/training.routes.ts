export class TrainingRoutes {
  public static BASE_PATH = 'training';

  public static COMPLIANCE_TRAINING = 'compliance-training';

  /**
   * @description
   * Useful for redirecting to module root-level routes.
   */
  public static routePath(route: string): string {
    return `/${TrainingRoutes.BASE_PATH}/${route}`;
  }
}
