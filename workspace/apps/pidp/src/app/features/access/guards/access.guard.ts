import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { catchError, map, of } from 'rxjs';

import { PartyService } from '@app/core/party/party.service';
import { StatusCode } from '@app/features/portal/enums/status-code.enum';
import { PortalResource } from '@app/features/portal/portal-resource.service';
import { AccessSectionKey } from '@app/features/portal/state/access/access-group.model';

export const accessGuard: CanActivateFn = (route) => {
  const partyService = inject(PartyService);
  const portalResource = inject(PortalResource);
  const router = inject(Router);

  if (!partyService.partyId) {
    return router.createUrlTree(['/']);
  }

  const accessModule = route.data['accessModule'] as AccessSectionKey;
  const allowedStatusCodes = route.data['allowedStatusCodes'] as StatusCode[];

  if (!accessModule || !allowedStatusCodes) {
    return router.createUrlTree(['/']);
  }

  return portalResource.getProfileStatus(partyService.partyId).pipe(
    map((profileStatus) => {
      if (!profileStatus) {
        return router.createUrlTree(['/']);
      }

      const statusCode = profileStatus.status[accessModule]?.statusCode;
      if (statusCode !== undefined && allowedStatusCodes.includes(statusCode)) {
        return true;
      }

      return router.createUrlTree(['/']);
    }),
    catchError(() => of(router.createUrlTree(['/']))),
  );
};
