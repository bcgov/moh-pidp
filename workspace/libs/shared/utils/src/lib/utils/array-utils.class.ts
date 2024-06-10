/* eslint-disable @typescript-eslint/no-explicit-any */
export class ArrayUtils {
  /**
   * @description
   * Conditional insert into an array when used in conjunction
   * with the spread operator.
   *
   * @example
   * const example = [1, 2, 3, ...ArrayUtils.insertIf(true, 4)] // [1, 2, 3, 4]
   * const example = [1, 2, 3, ...ArrayUtils.insertIf(false, 4)] // [1, 2, 3]
   */
  public static insertIf<T>(condition: boolean, ...elements: any[]): T[] {
    return condition ? [].concat(...elements) : [];
  }

  /**
   * @description
   * Conditional insert into an array by invoking a callback
   * used in conjunction with the spread operator.
   *
   * @example
   * const doSomething = () => [4, 5, 6]
   * const example = [1, 2, 3, ...ArrayUtils.insertResultIf(true, doSomething)] // [1, 2, 3, 4, 5, 6]
   * const example = [1, 2, 3, ...ArrayUtils.insertResultIf(false, doSomething)] // [1, 2, 3]
   */
  public static insertResultIf<T>(
    condition: boolean,
    callback: () => any[],
  ): T[] {
    return condition ? callback() : [];
  }

  /**
   * @description
   * Find the intersection between two arrays.
   */
  public static intersection<T>(arrX: T[], arrY: T[]): T[] {
    return arrX.filter((x) => arrY.includes(x));
  }

  /**
   * @description
   * Find the difference between two arrays.
   */
  public static difference<T>(arrX: T[], arrY: T[]): T[] {
    return arrX.filter((x) => !arrY.includes(x));
  }

  /**
   * @description
   * Find the union between two arrays.
   */
  public static union<T>(arrX: T[], arrY: T[]): T[] {
    return arrX.concat(arrY);
  }

  /**
   * @description
   * Find outer-section between two arrays.
   */
  public static symmetricDifference<T>(arrX: T[], arrY: T[]): T[] {
    return arrX
      .filter((x) => !arrY.includes(x))
      .concat(arrY.filter((y) => !arrX.includes(y)));
  }
}
