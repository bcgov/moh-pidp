import { Section } from '../section.model';

export enum Destination {
  DEMOGRAPHICS = 1,
  USER_ACCESS_AGREEMENT,
}

export interface RedirectSection extends Section {
  destination: Destination;
}
