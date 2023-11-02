import { Injectable } from '@angular/core';
import { Resolve, Router } from '@angular/router';
import { EMPTY, Observable, catchError, of, switchMap } from 'rxjs';
import { Destination, DiscoveryResource } from './discovery-resource.service';
import { PartyService } from './party.service';
import { ProfileRoutes } from '@app/features/profile/profile.routes';

@Injectable({
  providedIn: 'root',
})
export class DestinationResolver implements Resolve<Destination | null> {
  public constructor(
    private partyService: PartyService,
    private resource: DiscoveryResource,
    private router: Router,
  ) {}

  public resolve(): Observable<Destination | null> {
    if (!this.partyService.partyId) {
      return of(null);
    }

    return this.resource.getDestination(this.partyService.partyId).pipe(
      switchMap((destination) => {
        switch (destination) {
          case Destination.DEMOGRAPHICS:
            this.router.navigateByUrl(
              ProfileRoutes.routePath(ProfileRoutes.PERSONAL_INFO),
            );
            return EMPTY;
          case Destination.USER_ACCESS_AGREEMENT:
            this.router.navigateByUrl(
              ProfileRoutes.routePath(ProfileRoutes.USER_ACCESS_AGREEMENT),
            );
            return EMPTY;
          // TODO: soon
          // case Destination.LICENCE_DECLARATION:
          //   this.router.navigateByUrl(
          //     ProfileRoutes.routePath(
          //       ProfileRoutes.COLLEGE_LICENCE_DECLARATION,
          //     ),
          //   );
          //   return EMPTY;
          default:
            return of(destination);
        }
      }),
      catchError(() => of(null)),
    );
  }
}
