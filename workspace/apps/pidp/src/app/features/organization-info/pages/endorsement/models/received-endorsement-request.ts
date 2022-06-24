export interface ReceivedEndorsementRequest {
  id: number;
  partyName: string;
  jobTitle: string;
  approved?: boolean;
  adjudicatedOn?: string;
}
