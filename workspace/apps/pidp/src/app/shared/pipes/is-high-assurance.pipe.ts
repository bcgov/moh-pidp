import { Pipe, PipeTransform } from '@angular/core';

import { IdentityProvider } from '@app/features/auth/enums/identity-provider.enum';

@Pipe({
  name: 'isHighAssurance',
  standalone: true,
})
export class IsHighAssurancePipe implements PipeTransform {
  public transform(value: IdentityProvider | null): boolean {
    return (
      value === IdentityProvider.BCSC || value === IdentityProvider.BC_PROVIDER
    );
  }
}
