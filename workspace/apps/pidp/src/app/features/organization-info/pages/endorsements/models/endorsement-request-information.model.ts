import { EndorsementRequest } from './endorsement-request.model';

export interface EndorsementRequestInformation
  extends Pick<
    EndorsementRequest,
    'recipientEmail' | 'additionalInformation'
  > {}
