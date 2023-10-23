import { Injectable } from '@angular/core';

import { DashboardStateModel, PidpStateName } from '@pidp/data-model';
import { AppStateService } from '@pidp/presentation';

import { PartyService } from '@app/core/party/party.service';
import { ProfileStatus } from '@app/features/portal/models/profile-status.model';
import { PortalResource } from '@app/features/portal/portal-resource.service';
import { LookupService } from '@app/modules/lookup/lookup.service';

@Injectable({
  providedIn: 'root',
})
export class DashboardStateService {
  public constructor(
    private partyService: PartyService,
    private portalResourceService: PortalResource,
    private lookupService: LookupService,
    private stateService: AppStateService,
  ) {}

  public refreshDashboardState(): void {
    // Get profile status and college info.
    const partyId = this.partyService.partyId;

    // Use forkJoin to wait for both to return.
    this.portalResourceService
      .getProfileStatus(partyId)
      .subscribe((profileStatus) => {
        const displayFullName = this.getUserDisplayFullName(profileStatus);
        const collegeName = this.getCollegeName(profileStatus);

        // Set the user name and college on the dashboard.
        const oldState = this.stateService.getNamedState<DashboardStateModel>(
          PidpStateName.dashboard,
        );
        const newState: DashboardStateModel = {
          ...oldState,
          userProfileFullNameText: displayFullName,
          userProfileCollegeNameText: collegeName,
        };
        this.stateService.setNamedState(PidpStateName.dashboard, newState);
      });
  }

  private getUserDisplayFullName(profileStatus: ProfileStatus | null): string {
    return profileStatus?.status.dashboardInfo.displayFullName ?? '';
  }

  private getCollegeName(profileStatus: ProfileStatus | null): string {
    if (!profileStatus?.status?.dashboardInfo) {
      return '';
    }

    const collegeCode = profileStatus.status.dashboardInfo.collegeCode ?? null;
    if (!collegeCode) {
      return '';
    }

    const college = this.lookupService.getCollege(collegeCode);
    return college?.name ?? '';
  }
}
