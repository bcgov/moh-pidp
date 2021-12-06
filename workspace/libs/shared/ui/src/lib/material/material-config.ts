import { LuxonDateAdapter } from '@angular/material-luxon-adapter';
import {
  DateAdapter,
  MAT_DATE_FORMATS,
  MAT_DATE_LOCALE,
} from '@angular/material/core';
import { MAT_DIALOG_DEFAULT_OPTIONS } from '@angular/material/dialog';
import {
  MatFormFieldDefaultOptions,
  MAT_FORM_FIELD_DEFAULT_OPTIONS,
} from '@angular/material/form-field';
import { MAT_PAGINATOR_DEFAULT_OPTIONS } from '@angular/material/paginator';

const APP_DATE_FORMAT = 'D MMM YYYY';
const APP_DATE_FORMATS = {
  parse: {
    // Reformat entered date values to this format
    dateInput: APP_DATE_FORMAT,
  },
  display: {
    dateInput: APP_DATE_FORMAT,
    monthYearLabel: 'MMM YYYY',
    dateA11yLabel: APP_DATE_FORMAT,
    monthYearA11yLabel: 'MMM YYYY',
  },
};
const appPaginatorCustomOptions = {
  pageSize: 100,
  hidePageSize: true,
  showFirstLastButtons: true,
};

const matFormFieldCustomOptions: MatFormFieldDefaultOptions = {
  hideRequiredMarker: true,
  floatLabel: 'always',
};

export const materialConfig = [
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
      width: '500px',
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
