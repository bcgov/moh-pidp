import { Provider } from '@angular/core';
import { LuxonDateAdapter } from '@angular/material-luxon-adapter';
import {
  DateAdapter,
  MAT_DATE_FORMATS,
  MAT_DATE_LOCALE,
} from '@angular/material/core';
import { MAT_DIALOG_DEFAULT_OPTIONS } from '@angular/material/dialog';
import {
  MAT_FORM_FIELD_DEFAULT_OPTIONS,
  MatFormFieldDefaultOptions,
} from '@angular/material/form-field';
import { MAT_PAGINATOR_DEFAULT_OPTIONS } from '@angular/material/paginator';

export const APP_DATE_FORMAT = 'dd LLL yyyy';
export const APP_DATE_FORMATS = {
  parse: {
    // Date values reformatted to this format:
    dateInput: APP_DATE_FORMAT,
  },
  display: {
    dateInput: APP_DATE_FORMAT,
    monthYearLabel: 'LLL yyyy',
    dateA11yLabel: APP_DATE_FORMAT,
    monthYearA11yLabel: 'LLL yyyy',
  },
};

const appPaginatorCustomOptions = {
  pageSize: 100,
  hidePageSize: true,
  showFirstLastButtons: true,
};

const matFormFieldCustomOptions: MatFormFieldDefaultOptions = {
  appearance: 'outline',
  hideRequiredMarker: true,
  floatLabel: 'always',
};

export function provideMaterialConfig(): Provider[] {
  return [
    {
      provide: MAT_DATE_FORMATS,
      useValue: APP_DATE_FORMATS,
    },
    {
      provide: DateAdapter,
      useClass: LuxonDateAdapter,
      deps: [MAT_DATE_LOCALE],
    },
    {
      provide: MAT_DIALOG_DEFAULT_OPTIONS,
      useValue: {
        hasBackdrop: true,
      },
    },
    {
      provide: MAT_FORM_FIELD_DEFAULT_OPTIONS,
      useValue: matFormFieldCustomOptions,
    },
    {
      provide: MAT_PAGINATOR_DEFAULT_OPTIONS,
      useValue: appPaginatorCustomOptions,
    },
  ];
}
