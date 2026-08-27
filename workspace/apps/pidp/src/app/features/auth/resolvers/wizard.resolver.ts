import { inject } from '@angular/core';
import { ResolveFn, Router } from '@angular/router';

import { map } from 'rxjs';

import {
  Destination,
  DiscoveryResource,
} from '@app/core/party/discovery-resource.service';
import { PartyService } from '@app/core/party/party.service';
import { PortalRoutes } from '@app/features/portal/portal.routes';

export const wizardResolver: ResolveFn<
  boolean | null | Promise<boolean>
> = (route, state) => {
  const partyId = inject(PartyService).partyId;
  const discoveryResource = inject(DiscoveryResource);
  const router = inject(Router);
  return discoveryResource.getDestination(partyId).pipe(
    map((destination: Destination) => {
      if (destination === Destination.PORTAL) {
        return true;
      }

      const urlToSave = state?.url;
      if (urlToSave && urlToSave !== '/') {
        sessionStorage.setItem('return-url', urlToSave);
      }
      return router.navigateByUrl(PortalRoutes.BASE_PATH);
    }),
  );
};
