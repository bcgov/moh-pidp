import { inject } from '@angular/core';
import { ResolveFn } from '@angular/router';

import { catchError, map, of } from 'rxjs';

import { PartyService } from '@app/core/party/party.service';
import { StatusCode } from '@app/features/portal/enums/status-code.enum';
import { ProfileStatus } from '@app/features/portal/models/profile-status.model';

import { PemcodResource } from './pemcod-resource.service';

export const pemcodResolver: ResolveFn<StatusCode | null> = () => {
  const partyService = inject(PartyService);
  const resource = inject(PemcodResource);

  if (!partyService.partyId) {
    return of(null);
  }

  return resource.getProfileStatus(partyService.partyId).pipe(
    map((profileStatus: ProfileStatus | null) => {
      if (!profileStatus) {
        return null;
      }

      return profileStatus.status.pemcod.statusCode;
    }),
    catchError(() => of(null)),
  );
};
