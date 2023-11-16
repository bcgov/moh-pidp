import { inject } from '@angular/core';
import { ResolveFn, Router } from '@angular/router';
import { PartyService } from '@app/core/party/party.service';
import { AccessRoutes } from '@app/features/access/access.routes';
import { StatusCode } from '@app/features/portal/enums/status-code.enum';
import { ProfileStatus } from '@app/features/portal/models/profile-status.model';
import { PortalResource } from '@app/features/portal/portal-resource.service';
import { map } from 'rxjs';

export const bcProviderCompletedResolver: ResolveFn<
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
        profileStatus.status.bcProvider.statusCode === StatusCode.AVAILABLE ||
        router.navigateByUrl(
          `${AccessRoutes.MODULE_PATH}/${AccessRoutes.BC_PROVIDER_EDIT}`,
        )
      );
    }),
  );
};
