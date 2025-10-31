export interface ExternalAccountsFormData {
  userPrincipalName: string;
}

export enum InvitationSteps {
  DOMAINS = 1,
  USER_PRINCIPAL_NAME,
  EMAIL_VERIFICATION,
  COMPLETED,
}
