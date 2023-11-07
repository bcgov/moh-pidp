import { Injectable } from '@angular/core';
import { Resolve, Router } from '@angular/router';
import { Observable, catchError, of, tap } from 'rxjs';
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

  private wizardComplete = false;

  public resolve(): Observable<Destination | null> {
    if (!this.partyService.partyId) {
      return of(null);
    }

    if (this.wizardComplete) {
      return of(Destination.PORTAL);
    }

    return this.resource.getDestination(this.partyService.partyId).pipe(
      tap((destination) => {
        switch (destination) {
          case Destination.DEMOGRAPHICS:
            this.router.navigateByUrl(
              ProfileRoutes.routePath(ProfileRoutes.PERSONAL_INFO),
            );
            break;
          case Destination.USER_ACCESS_AGREEMENT:
            this.router.navigateByUrl(
              ProfileRoutes.routePath(ProfileRoutes.USER_ACCESS_AGREEMENT),
            );
            break;
          // TODO: soon
          // case Destination.LICENCE_DECLARATION:
          //   this.router.navigateByUrl(
          //     ProfileRoutes.routePath(
          //       ProfileRoutes.COLLEGE_LICENCE_DECLARATION,
          //     ),
          //   );
          case Destination.PORTAL:
            this.wizardComplete = true;
            break;
        }
      }),
      catchError(() => of(null)),
    );
  }
}
