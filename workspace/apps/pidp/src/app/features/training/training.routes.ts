export class TrainingRoutes {
  public static readonly BASE_PATH = 'training';

  public static readonly COMPLIANCE_TRAINING = 'compliance-training';

  /**
   * @description
   * Useful for redirecting to module root-level routes.
   */
  public static routePath(route: string): string {
    return `/${TrainingRoutes.BASE_PATH}/${route}`;
  }
}
