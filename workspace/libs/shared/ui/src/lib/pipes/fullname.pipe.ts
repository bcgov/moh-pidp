import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'fullname',
  standalone: true,
})
export class FullnamePipe implements PipeTransform {
  public transform(
    model:
      | {
          firstName?: string;
          lastName: string;
          // eslint-disable-next-line @typescript-eslint/no-explicit-any
          [key: string]: any;
        }
      | null
      | undefined,
  ): string | null {
    if (!model) {
      return null;
    }

    const { firstName, lastName } = model;
    return firstName && lastName ? `${firstName} ${lastName}` : '';
  }
}
