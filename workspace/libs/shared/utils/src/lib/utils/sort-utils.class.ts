import { SortDirection } from '@angular/material/sort';

export type SortWeight = -1 | 0 | 1;

export class SortUtils {
  /**
   * @description
   * Generic sorting of a JSON object by key.
   */
  public static sort<T>(a: T, b: T): SortWeight {
    return a > b ? 1 : a < b ? -1 : 0;
  }

  /**
   * @description
   * Generic sorting of a JSON object by key.
   */
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  public static sortByKey<T extends { [key: string]: any }>(
    key: keyof T
  ): (a: T, b: T) => SortWeight {
    return (a: T, b: T): SortWeight => this.sort<T>(a[key], b[key]);
  }

  /**
   * @description
   * Generic sorting of a JSON object by direction.
   */
  public static sortByDirection<T>(
    a: T,
    b: T,
    direction: SortDirection = 'asc',
    withTrailingNull: boolean = true
  ): SortWeight {
    let result: SortWeight;

    if (a === null && withTrailingNull) {
      result = -1;
    } else if (b === null && withTrailingNull) {
      result = 1;
    } else {
      result = this.sort(a, b);
    }

    if (direction === 'desc') {
      result *= -1;
    }

    return result as SortWeight;
  }
}
