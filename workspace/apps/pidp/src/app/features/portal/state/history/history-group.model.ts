/**
 * @description
 * Section keys as a readonly tuple to allow iteration
 * over keys at runtime to allow filtering or grouping
 * sections.
 */
export const historySectionKeys = [
  'signedAcceptedDocuments',
  'transactions',
] as const;

/**
 * @description
 * Union of keys generated from the tuple.
 */
export type HistorySectionKey = (typeof historySectionKeys)[number];

// TODO not implemented since the history group is
// different from the other groups as its sections do
// not have a status. Waiting to see what happens.

/**
 * @description
 * Typing for a group generated from a union.
 */
// export type IHistoryGroup = {
//   [K in HistorySectionKey]: Section;
// };

// export interface HistoryGroup extends IHistoryGroup {}
