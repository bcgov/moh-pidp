import { ExternalGuestInvitation } from "@bcgov/shared/data-access";

/* eslint-disable @typescript-eslint/no-empty-interface */
export interface ExternalGuestInvitationInformation
  extends Pick<ExternalGuestInvitation, 'username'> {}
