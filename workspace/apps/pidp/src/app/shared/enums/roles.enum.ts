export enum Role {
  /**
   * @description
   * Assumed that if you're authenticated that you are
   * a USER unless you have the ADMIN role.
   *
   * @deprecated
   */
  USER = 'USER',
  ADMIN = 'ADMIN',
  FEATURE_PIDP_DEMO = 'feature_pidp_demo',
}
