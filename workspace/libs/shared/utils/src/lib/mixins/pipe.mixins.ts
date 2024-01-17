/* eslint-disable @typescript-eslint/no-explicit-any */
import { PipeTransform } from '@angular/core';

export function fromFunction<T extends (...args: any[]) => any>(
  transform: T,
): new () => PipeTransform {
  return class {
    public transform(value: any, ...params: Parameters<T>): ReturnType<T> {
      return transform(value, ...params);
    }
  };
}
