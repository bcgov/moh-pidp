/* eslint-disable @typescript-eslint/no-empty-interface */
import { EndorsementRequest } from './endorsement-request.model';

export type EndorsementRequestInformation = Pick<
  EndorsementRequest,
  'recipientEmail' | 'additionalInformation' | 'preApproved'
>;
