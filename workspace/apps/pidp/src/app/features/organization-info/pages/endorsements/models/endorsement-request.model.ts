import { EndorsementRequestStatus } from '../enums/endorsement-request-status.enum';

export interface EndorsementRequest {
  id: number;
  recipientEmail: string;
  additionalInformation: string | null;
  partyName: string | null;
  collegeCode: number | null;
  status: EndorsementRequestStatus;
  statusDate: string;
  actionable: boolean;
}

export interface EndorsementRequestInformation {
  recipientEmails: string;
  additionalInformation: string | null;
}

export interface EndorsementRequestCommand {
  recipientEmails: string[];
  additionalInformation: string | null;
}
