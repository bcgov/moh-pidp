import { Pipe, PipeTransform } from '@angular/core';

import { DateTime } from 'luxon';

import { APP_DATE_FORMAT } from '../material/material-config';

@Pipe({
  name: 'formatDate',
  standalone: true,
})
export class FormatDatePipe implements PipeTransform {
  public transform(
    date: string | null | undefined,
    format: string = APP_DATE_FORMAT,
  ): string | null {
    if (!date) {
      return null;
    }

    return DateTime.fromISO(date).toFormat(format);
  }
}
