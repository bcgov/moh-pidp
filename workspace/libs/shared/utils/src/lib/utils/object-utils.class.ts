/* eslint-disable @typescript-eslint/no-explicit-any */

export class ObjectUtils {
  /**
   * @description
   * Map an object's keys by reference.
   */
  public static keyMapping(
    object: { [key: string]: any },
    mapping: { [key: string]: string },
  ): void {
    if (!object || !mapping) {
      return;
    }

    Object.keys(object).forEach((oldKey) => {
      const newKey = mapping[oldKey];
      if (newKey) {
        object[newKey] = object[oldKey];
        delete object[oldKey];
      }
    });
  }

  /**
   * @description
   * Map an object's keys by value.
   */
  public static keyMappingImmutable(
    object: { [key: string]: any },
    mapping: { [key: string]: string },
  ): { [key: string]: unknown } | null {
    if (!object || !mapping) {
      return null;
    }

    return Object.keys({ ...object }).reduce(
      (mapped: { [key: string]: unknown }, key: string) => (
        (mapped[mapping[key] ?? key] = object[key]), mapped
      ),
      {} as { [key: string]: unknown },
    );
  }

  /**
   * @description
   * Convert an array of strings to keys of an object literal.
   */
  public static toObjectKeys<T>(
    array: string[],
    defaultValue?: T,
  ): { [key: string]: T | null } {
    return array.reduce(
      (object, key: string) => ((object[key] = defaultValue ?? null), object),
      {} as { [key: string]: T | null },
    );
  }

  /**
   * @description
   * Merge a key/value pair into an object if the key
   * exists in the reference (source) object.
   */
  public static mergeInto(
    key: string,
    refObject: { [key: string]: any },
    mergeObject: { [key: string]: any } = {},
  ): { [key: string]: any } {
    if (!key || !refObject || !mergeObject) {
      return mergeObject;
    }

    return Object.hasOwnProperty.call(refObject, key)
      ? { ...mergeObject, [key]: refObject[key] }
      : mergeObject;
  }

  /**
   * @description
   * Merge a key/value pair into an object if the key
   * exists in the reference (source) object.
   */
  public static mergeIntoIf(
    condition: boolean,
    key: string,
    refObject: { [key: string]: any },
    mergeObject: { [key: string]: any } = {},
  ): { [key: string]: any } {
    return condition
      ? this.mergeInto(key, refObject, mergeObject)
      : mergeObject;
  }

  /**
   * @description
   * Recursively make an object immutable.
   *
   * NOTE: Only for use with simple objects to avoid the possibility of
   * an infinite loop being triggered, or freezing object that should
   * not be made immutable.
   */
  public static deepFreeze(object: any): { [key: string]: any } {
    // Retrieve the property names defined on object
    const propNames = Object.getOwnPropertyNames(object);

    // Freeze properties before freezing self
    for (const name of propNames) {
      const value = object[name];

      if (value && typeof value === 'object') {
        this.deepFreeze(value);
      }
    }

    return Object.freeze(object);
  }
}
