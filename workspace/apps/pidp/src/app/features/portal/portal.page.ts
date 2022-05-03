import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { Observable, map } from 'rxjs';

import { PartyService } from '@app/core/party/party.service';
import { SupportProvided } from '@app/shared/components/get-support/get-support.component';
import { Role } from '@app/shared/enums/roles.enum';

import { StatusCode } from './enums/status-code.enum';
import { ProfileStatusAlert } from './models/profile-status-alert.model';
import { ProfileStatus } from './models/profile-status.model';
import { PortalResource } from './portal-resource.service';
import { PortalService } from './portal.service';
import { accessSectionKeys } from './state/access/access-group.model';
import { PortalSectionStatusKey } from './state/portal-section-status-key.type';
import { IPortalSection } from './state/portal-section.model';
import { PortalState } from './state/portal-state.builder';
import { profileSectionKeys } from './state/profile/profile-group.model';

@Component({
  selector: 'app-portal',
  templateUrl: './portal.page.html',
  styleUrls: ['./portal.page.scss'],
})
export class PortalPage implements OnInit {
  public state$: Observable<PortalState>;
  /**
   * @description
   * Whether to show the profile information completed
   * alert providing a scrollable route to access requests.
   */
  public completedProfile: boolean;
  public alerts: ProfileStatusAlert[];
  public hiddenSupport: SupportProvided[];

  public Role = Role;

  public constructor(
    private router: Router,
    private partyService: PartyService,
    private portalResource: PortalResource,
    private portalService: PortalService
  ) {
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
          this.completedProfile = this.hasCompletedProfile(profileStatus);
          this.alerts = this.portalService.alerts;
          const filter: PortalSectionStatusKey[] = [
            'saEforms',
            'hcimAccountTransfer',
          ];
          this.hiddenSupport = this.portalService.hiddenSections.filter(
            (hiddenSection) => filter.includes(hiddenSection)
          ) as SupportProvided[];
        })
      )
      .subscribe();
  }

  /**
   * @description
   * Whether all profile information has been completed, and
   * no access requests have been made.
   */
  private hasCompletedProfile(profileStatus: ProfileStatus | null): boolean {
    if (!profileStatus) {
      return false;
    }

    const status = profileStatus.status;

    // Assumes key absence is a requirement and should be
    // purposefully skipped in the profile completed check
    return (
      profileSectionKeys.every((key) =>
        status[key] ? status[key].statusCode === StatusCode.COMPLETED : true
      ) &&
      accessSectionKeys.every((key) =>
        status[key] ? status[key].statusCode !== StatusCode.COMPLETED : true
      )
    );
  }
}
