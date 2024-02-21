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
> = () => {
  const partyId = inject(PartyService).partyId;
  const discoveryResource = inject(DiscoveryResource);
  const router = inject(Router);
  return discoveryResource.getDestination(partyId).pipe(
    map((destination: Destination) => {
      return (
        destination === Destination.PORTAL ||
        router.navigateByUrl(PortalRoutes.BASE_PATH)
      );
    }),
  );
};
