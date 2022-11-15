import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { Observable, map, of, switchMap } from 'rxjs';

import { PartyService } from '@app/core/party/party.service';
import { Role } from '@app/shared/enums/roles.enum';

import { EndorsementsResource } from '../organization-info/pages/endorsements/endorsements-resource.service';
import { ProfileStatusAlert } from './models/profile-status-alert.model';
import { ProfileStatus } from './models/profile-status.model';
import { PortalResource } from './portal-resource.service';
import { PortalService } from './portal.service';
import { IPortalSection } from './state/portal-section.model';
import { PortalState } from './state/portal-state.builder';

@Component({
  selector: 'app-portal',
  templateUrl: './portal.page.html',
  styleUrls: ['./portal.page.scss'],
})
export class PortalPage implements OnInit {
  /**
   * @description
   * State for driving the displayed groups and sections of
   * the portal.
   */
  public state$: Observable<PortalState>;
  /**
   * @description
   * List of HTTP response controlled alert messages for display
   * in the portal.
   */
  public alerts: ProfileStatusAlert[];
  /**
   * @description
   * Whether to show the profile information completed
   * alert providing a scrollable route to access requests.
   */
  public completedProfile: boolean;

  public Role = Role;

  public constructor(
    private router: Router,
    private partyService: PartyService,
    private portalResource: PortalResource,
    private portalService: PortalService,
    private endorsementsResource: EndorsementsResource,
    private activatedRoute: ActivatedRoute
  ) {
    this.state$ = this.portalService.state$;
    this.completedProfile = false;
    this.alerts = [];
  }

  public onScrollToAnchor(): void {
    this.router.navigate([], {
      fragment: 'access',
      queryParamsHandling: 'preserve',
    });
  }

  public onCardAction(section: IPortalSection): void {
    section.performAction();
  }

  public ngOnInit(): void {
    this.handleLandingActions$()
      .pipe(
        switchMap(() =>
          this.portalResource.getProfileStatus(this.partyService.partyId).pipe(
            map((profileStatus: ProfileStatus | null) => {
              this.portalService.updateState(profileStatus);
              this.completedProfile = this.portalService.completedProfile;
              this.alerts = this.portalService.alerts;
            })
          )
        )
      )
      .subscribe();
  }

  public handleLandingActions$(): Observable<void> {
    const endorsementToken =
      this.activatedRoute.snapshot.queryParamMap.get('endorsement-token');

    if (!endorsementToken) {
      return of(undefined);
    }

    return this.endorsementsResource
      .receiveEndorsementRequest(this.partyService.partyId, endorsementToken)
      .pipe(
        map(() => {
          this.router.navigate([], {
            queryParams: {
              'endorsement-token': null,
            },
            queryParamsHandling: 'merge',
          });
        })
      );
  }
}
