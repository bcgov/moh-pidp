import { EndorsementRequestStatus } from '../enums/endorsement-request-status.enum';

/**
 * @description
 * Mapping of endorsement request statuses to their respective labels.
 * The text can be 'Cancelled', 'Declined', 'In progress', or 'Requested'.
 */
export const endorsementRequestsLabelText: Partial<
  Record<EndorsementRequestStatus, string>
> = {
  [EndorsementRequestStatus.CANCELLED]: 'Cancelled',
  [EndorsementRequestStatus.DECLINED]: 'Declined',
  [EndorsementRequestStatus.APPROVED]: 'In progress',
};

/**
 * @description
 * List of endorsement request statuses that
 * have a red colored label to denote they are no longer active.
 */
export const endorsementRequestsRedStatus: EndorsementRequestStatus[] = [
  EndorsementRequestStatus.CANCELLED,
  EndorsementRequestStatus.DECLINED,
];
