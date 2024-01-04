/**
 * @description
 * Section keys as a readonly tuple to allow iteration
 * over keys at runtime to allow filtering or grouping
 * sections.
 */
export const faqSectionKeys = ['mfaSetup'] as const;

/**
 * @description
 * Union of keys generated from the tuple.
 */
export type FaqSectionKey = (typeof faqSectionKeys)[number];
