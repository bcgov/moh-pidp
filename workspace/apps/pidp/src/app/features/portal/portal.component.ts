import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { exhaustMap, map, of } from 'rxjs';

import { Party } from '@bcgov/shared/data-access';
import { AlertType } from '@bcgov/shared/ui';

import { PartyResource } from '@core/resources/party-resource.service';
import { PartyService } from '@core/services/party.service';

import { ShellRoutes } from '../shell/shell.routes';

export interface PortalSection {
  icon: string;
  type: string;
  title: string;
  description: string;
  process: 'manual' | 'automatic';
  hint?: string;
  actionLabel?: string;
  route?: string;
  statusType?: AlertType;
  status?: string;
  disabled: boolean;
}

// TODO find a clean way to type narrow in the template
@Component({
  selector: 'app-portal',
  templateUrl: './portal.component.html',
  styleUrls: ['./portal.component.scss'],
})
export class PortalComponent implements OnInit {
  public title: string;
  public acceptedCollectionNotice: boolean;
  public state: Record<string, PortalSection[]>;
  public completedProfile: boolean;

  public constructor(
    private route: ActivatedRoute,
    private router: Router,
    private partyResource: PartyResource,
    private partyService: PartyService
  ) {
    this.title = this.route.snapshot.data.title;

    this.acceptedCollectionNotice = this.partyService.acceptedCollectionNotice;
    this.completedProfile = this.partyService.completedProfile;
    this.state = this.partyService.state;
  }

  public onAcceptCollectionNotice(accepted: boolean): void {
    this.partyService.acceptedCollectionNotice = accepted;
  }

  public onAction(routePath?: string): void {
    if (!routePath) {
      return;
    }

    this.router.navigate([ShellRoutes.routePath(routePath)]);
  }

  public ngOnInit(): void {
    // TODO allow creation of a single party with ID 1
    const { firstName, lastName } = this.partyService.user;
    this.partyResource
      .getParties()
      .pipe(
        map((parties: Party[] | null) => (parties?.length ? parties[0] : null)),
        exhaustMap((party: Party | null) =>
          !party
            ? this.partyResource.createParty({ firstName, lastName }).pipe(
                exhaustMap(() => this.partyResource.getParties()),
                map((parties: Party[] | null) =>
                  parties?.length ? parties[0] : null
                )
              )
            : of(party)
        ),
        map((party: Party | null) => (this.partyService.party = party))
      )
      .subscribe();
  }
}
