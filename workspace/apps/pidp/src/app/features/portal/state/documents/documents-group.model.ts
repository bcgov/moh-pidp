/**
 * @description
 * Section keys as a readonly tuple to allow iteration
 * over keys at runtime to allow filtering or grouping
 * sections.
 */
export const documentSectionKeys = [
  'signedAcceptedDocuments',
  'transactions',
] as const;

/**
 * @description
 * Union of keys generated from the tuple.
 */
export type DocumentsSectionKey = typeof documentSectionKeys[number];

// TODO not implemented since the documents group is
// different from the other groups as its sections do
// not have a status. Waiting to see what happens.

/**
 * @description
 * Typing for a group generated from a union.
 */
// export type IDocumentGroup = {
//   [K in DocumentsSectionKey]: Section;
// };

/**
 * @description
 * Type used to ensure adding a new key to the tuple is
 * included in the group interface.
 */
// eslint-disable-next-line @typescript-eslint/no-unused-vars
// type CheckGroup<T extends IDocumentGroup = DocumentGroup> = void;

// eslint-disable-next-line @typescript-eslint/no-empty-interface
// export interface DocumentGroup {}
