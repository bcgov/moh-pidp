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
  preApproved: boolean;
}
