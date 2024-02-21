import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'postal',
  standalone: true,
})
export class PostalPipe implements PipeTransform {
  public transform(value: string | null | undefined): string | null {
    return value ? this.postalValue(value) : null;
  }

  private postalValue(value: string): string {
    return `${value.toUpperCase().slice(0, 3)} ${value.toUpperCase().slice(3)}`;
  }
}
