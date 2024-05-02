import { EndorsementRequestStatus } from '../enums/endorsement-request-status.enum';

export const endorsementRequestsLabelText: Partial<
  Record<EndorsementRequestStatus, string>
> = {
  [EndorsementRequestStatus.CANCELLED]: 'Cancelled',
  [EndorsementRequestStatus.DECLINED]: 'Declined',
  [EndorsementRequestStatus.APPROVED]: 'In progress',
};
export const endorsementRequestsRedStatus: EndorsementRequestStatus[] = [
  EndorsementRequestStatus.CANCELLED,
  EndorsementRequestStatus.DECLINED,
];
