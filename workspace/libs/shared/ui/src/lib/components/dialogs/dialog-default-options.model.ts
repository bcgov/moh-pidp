import { DialogOptions } from './dialog-options.model';

export interface DialogDefaultOptions {
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  [key: string]: (...args: any) => DialogOptions;
}
