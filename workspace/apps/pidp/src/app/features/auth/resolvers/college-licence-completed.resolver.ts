import { inject } from '@angular/core';
import { ResolveFn, Router } from '@angular/router';

import { map } from 'rxjs';

import { PartyService } from '@app/core/party/party.service';
import { ProfileStatus } from '@app/features/portal/models/profile-status.model';
import { PortalResource } from '@app/features/portal/portal-resource.service';
import { PortalRoutes } from '@app/features/portal/portal.routes';

export const collegeLicenceCompletedResolver: ResolveFn<
  boolean | null | Promise<boolean>
> = () => {
  const partyId = inject(PartyService).partyId;
  const portalResource = inject(PortalResource);
  const router = inject(Router);
  return portalResource.getProfileStatus(partyId).pipe(
    map((profileStatus: ProfileStatus | null) => {
      if (!profileStatus) {
        return null;
      }

      return (
        !profileStatus.status.collegeCertification.hasCpn ||
        router.navigateByUrl(PortalRoutes.BASE_PATH)
      );
    }),
  );
};
