import { HistorySectionKey } from './history/history-group.model';
import { PortalSectionStatusKey } from './portal-section-status-key.type';

/**
 * @description
 * Set of unique identifiers for all possible
 * portal sections.
 */
export type PortalSectionKey =
  | PortalSectionStatusKey
  // Status-less sections are listed to allow
  // their typed inclusion
  | HistorySectionKey;
