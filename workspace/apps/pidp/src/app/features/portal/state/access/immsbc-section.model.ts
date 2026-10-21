import { Section } from '../section.model';

export interface ImmsBcSection extends Section {
  isLead: boolean;
  isEndUser: boolean;
  isPending: boolean;
}
