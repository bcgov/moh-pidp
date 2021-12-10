import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'configCode',
})
export class ConfigCodePipe implements PipeTransform {
  // TODO setup configuration module, service, and model
  public transform(value: unknown, ...args: any): unknown {
    return value;
  }
}
