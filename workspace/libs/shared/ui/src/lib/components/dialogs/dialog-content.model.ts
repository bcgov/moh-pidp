import { EventEmitter } from '@angular/core';

export interface IDialogContent {
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  data: any;
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  output?: EventEmitter<any>;
}
