import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { CanActivateFn } from '@angular/router';

import { AuthorizedUserService } from '@app/features/auth/services/authorized-user.service';
import { Role } from '@app/shared/enums/roles.enum';

export const adminGuard: CanActivateFn = () => {
  const router = inject(Router);
  const authorizedUserService = inject(AuthorizedUserService);

  return authorizedUserService.roles.includes(Role.ADMIN)
    ? true
    : router.createUrlTree(['/']);
};
