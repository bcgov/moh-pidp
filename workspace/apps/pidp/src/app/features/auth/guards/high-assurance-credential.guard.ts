import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';

import { map } from 'rxjs';

import { IdentityProvider } from '../enums/identity-provider.enum';
import { AuthorizedUserService } from '../services/authorized-user.service';

export const highAssuranceCredentialGuard: CanActivateFn = () => {
  const router = inject(Router);
  const authorizedUserService = inject(AuthorizedUserService);

  return authorizedUserService.identityProvider$.pipe(
    map((identityProvider: IdentityProvider) => {
      return identityProvider === IdentityProvider.BCSC ||
        identityProvider === IdentityProvider.BC_PROVIDER
        ? true
        : router.createUrlTree(['/']);
    }),
  );
};
