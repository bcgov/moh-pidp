import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'default',
  standalone: true,
})
export class DefaultPipe implements PipeTransform {
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  public transform(value: any, defaultValue = '-'): any {
    if (typeof value === 'string') {
      value = value.trim();
    }
    return value ? value : defaultValue;
  }
}
