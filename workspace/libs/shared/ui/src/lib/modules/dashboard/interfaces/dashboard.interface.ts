export interface IDashboard {
  /**
   * @description
   * Modules specific logout URL.
   */
  logoutRedirectUrl: string;
  /**
   * @description
   * Logout event handler.
   */
  onLogout: () => void;
}
