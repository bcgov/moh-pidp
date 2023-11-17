import { inject } from '@angular/core';

import { catchError, map, of } from 'rxjs';

import { PartyService } from '@app/core/party/party.service';
import { StatusCode } from '@app/features/portal/enums/status-code.enum';
import { ProfileStatus } from '@app/features/portal/models/profile-status.model';

import { MsTeamsPrivacyOfficerResource } from './ms-teams-privacy-officer-resource.service';
import { ResolveFn } from '@angular/router';

export const msTeamsPrivacyOfficerResolver: ResolveFn<
  StatusCode | null
> = () => {
  const partyService = inject(PartyService);
  const resource = inject(MsTeamsPrivacyOfficerResource);

  if (!partyService.partyId) {
    return of(null);
  }

  return resource.getProfileStatus(partyService.partyId).pipe(
    map((profileStatus: ProfileStatus | null) => {
      if (!profileStatus) {
        return null;
      }

      return profileStatus.status.msTeamsPrivacyOfficer.statusCode;
    }),
    catchError(() => of(null)),
  );
};
