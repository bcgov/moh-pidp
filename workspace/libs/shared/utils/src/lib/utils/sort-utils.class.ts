export type SortWeight = -1 | 0 | 1;

export class SortUtils {
  /**
   * @description
   * Predicate for generic sorting.
   */
  public static sort<T>(a: T, b: T): SortWeight {
    return a > b ? 1 : a < b ? -1 : 0;
  }

  /**
   * @description
   * Predicate for generic sorting of a JSON object by key.
   */
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  public static sortByKey<T extends { [key: string]: any }>(
    key: keyof T,
  ): (a: T, b: T) => SortWeight {
    return (a: T, b: T): SortWeight => this.sort<T>(a[key], b[key]);
  }
}
