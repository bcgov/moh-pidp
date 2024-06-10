import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';

import { APP_CONFIG } from '@app/app.config';
import { AuthorizedUserService } from '@app/features/auth/services/authorized-user.service';
import { Role } from '@app/shared/enums/roles.enum';

export const userGuard: CanActivateFn = () => {
  const config = inject(APP_CONFIG);
  const router = inject(Router);
  const authorizedUserService = inject(AuthorizedUserService);

  return authorizedUserService.roles.includes(Role.ADMIN)
    ? router.createUrlTree([config.routes.admin])
    : true;
};
