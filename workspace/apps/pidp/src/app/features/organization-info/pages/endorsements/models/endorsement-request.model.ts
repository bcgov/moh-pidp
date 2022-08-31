import { EndorsementRequestStatus } from '../enums/endorsement-request-status.enum';

export interface EndorsementRequest {
  id: number;
  recipientEmail: string;
  partyName: string | null;
  status: EndorsementRequestStatus;
  statusDate: string;
  actionable: boolean;
}
