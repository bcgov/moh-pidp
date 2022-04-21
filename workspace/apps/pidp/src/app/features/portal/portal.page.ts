import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { Observable, map } from 'rxjs';

import { PartyService } from '@app/core/party/party.service';
import { SupportProvided } from '@app/shared/components/get-support/get-support.component';
import { Role } from '@app/shared/enums/roles.enum';

import { PortalResource } from './portal-resource.service';
import { PortalService } from './portal.service';
import { IPortalSection } from './sections/classes';
import { ProfileStatusAlert } from './sections/models/profile-status-alert.model';
import { ProfileStatus } from './sections/models/profile-status.model';

@Component({
  selector: 'app-portal',
  templateUrl: './portal.page.html',
  styleUrls: ['./portal.page.scss'],
})
export class PortalPage implements OnInit {
  public title: string;
  public state$: Observable<Record<string, IPortalSection[]>>;
  public completedProfile: boolean;
  public alerts: ProfileStatusAlert[];
  public hiddenSupport: SupportProvided[];

  public Role = Role;

  public constructor(
    private route: ActivatedRoute,
    private router: Router,
    private partyService: PartyService,
    private portalResource: PortalResource,
    private portalService: PortalService
  ) {
    this.title = this.route.snapshot.data.title;
    this.state$ = this.portalService.state$;
    this.completedProfile = false;
    this.alerts = [];
    this.hiddenSupport = [];
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
    this.portalResource
      .getProfileStatus(this.partyService.partyId)
      .pipe(
        map((profileStatus: ProfileStatus | null) => {
          this.portalService.updateState(profileStatus);
          this.completedProfile = this.portalService.completedProfile;
          this.alerts = this.portalService.alerts;
          this.hiddenSupport = this.portalService.hiddenSections.filter(
            (hiddenSection) => ['saEforms', 'hcim'].includes(hiddenSection)
          ) as SupportProvided[];
        })
      )
      .subscribe();
  }
}
