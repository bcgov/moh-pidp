import { Injectable } from '@angular/core';

import {
  DashboardStateModel,
  PidpStateName,
} from '@app/features/portal/models/state.model';

import { AppStateService } from './app-state.service';

@Injectable({
  providedIn: 'root',
})
export class ApplicationService {
  public constructor(private stateService: AppStateService) {}
  public setDashboardTitleText(titleText: string, subtitleText: string): void {
    const oldState = this.stateService.getNamedState<DashboardStateModel>(
      PidpStateName.dashboard,
    );
    const newState: DashboardStateModel = {
      ...oldState,
      titleText: titleText,
      titleDescriptionText: subtitleText,
    };
    this.stateService.setNamedState(PidpStateName.dashboard, newState);
  }
}
