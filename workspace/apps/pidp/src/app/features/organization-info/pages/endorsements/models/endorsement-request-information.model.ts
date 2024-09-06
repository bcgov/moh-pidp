/* eslint-disable @typescript-eslint/no-empty-interface */
import { EndorsementRequest } from './endorsement-request.model';

export interface EndorsementRequestInformation
  extends Pick<
    EndorsementRequest,
    'recipientEmail' | 'additionalInformation' | 'preApproved'
  > { }
