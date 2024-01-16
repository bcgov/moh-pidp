import { inject } from '@angular/core';
import { ResolveFn, Router } from '@angular/router';

import { catchError, of, tap } from 'rxjs';

import { ProfileRoutes } from '@app/features/profile/profile.routes';

import { Destination, DiscoveryResource } from './discovery-resource.service';
import { PartyService } from './party.service';

export const destinationResolver: ResolveFn<Destination | null> = () => {
  const partyService = inject(PartyService);
  const resource = inject(DiscoveryResource);
  const router = inject(Router);

  let wizardComplete = false;

  if (!partyService.partyId) {
    return of(null);
  }

  if (wizardComplete) {
    return of(Destination.PORTAL);
  }

  return resource.getDestination(partyService.partyId).pipe(
    tap((destination) => {
      switch (destination) {
        case Destination.DEMOGRAPHICS:
          router.navigateByUrl(
            ProfileRoutes.routePath(ProfileRoutes.PERSONAL_INFO),
          );
          break;
        case Destination.USER_ACCESS_AGREEMENT:
          router.navigateByUrl(
            ProfileRoutes.routePath(ProfileRoutes.USER_ACCESS_AGREEMENT),
          );
          break;
        case Destination.LICENCE_DECLARATION:
          router.navigateByUrl(
            ProfileRoutes.routePath(ProfileRoutes.COLLEGE_LICENCE_DECLARATION),
          );
          break;
        case Destination.PORTAL:
          wizardComplete = true;
          break;
      }
    }),
    catchError(() => of(null)),
  );
};
