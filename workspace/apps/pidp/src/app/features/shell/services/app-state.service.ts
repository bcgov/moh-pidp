import { Injectable } from '@angular/core';

import { BehaviorSubject, Observable, map } from 'rxjs';

import {
  ApplicationStateModel,
  NamedState,
  defaultApplicationState,
} from '@app/features/portal/models/state.model';

@Injectable({
  providedIn: 'root',
})
export class AppStateService {
  private state: ApplicationStateModel = defaultApplicationState;
  private stateSubject = new BehaviorSubject<ApplicationStateModel>(this.state);
  public stateBroadcast$ = this.stateSubject.asObservable();

  public getState(): ApplicationStateModel {
    return this.state;
  }
  public setState(state: ApplicationStateModel): void {
    this.state = { ...state };
    this.stateSubject.next(this.state);
  }
  public getNamedState<TNamedState extends NamedState>(
    name: string,
  ): TNamedState {
    const state = this.getState();
    const childState = state.all.find((x) => x.stateName === name);
    if (!childState) {
      throw new Error(`unknown named state '${name}'`);
    }
    const typedState = childState as TNamedState;
    return typedState;
  }
  public setNamedState<TNamedState extends NamedState>(
    name: string,
    state: TNamedState,
  ): void {
    const appState = this.getState();

    const appStateAsJson = JSON.stringify(appState);
    const newAppState: ApplicationStateModel = JSON.parse(
      appStateAsJson,
    ) as ApplicationStateModel;

    const index = appState.all.findIndex((x) => x.stateName === name);
    if (index === -1) {
      throw new Error(`cannot find named state '${name}'`);
    }

    newAppState.all.splice(index, 1, { ...state });

    this.setState(newAppState);
  }
  public getNamedStateBroadcast<TNamedState extends NamedState>(
    name: string,
  ): Observable<TNamedState> {
    return this.stateBroadcast$.pipe(
      map((state) => {
        const namedState = state.all.find((x) => x.stateName === name);
        if (!namedState) {
          throw new Error(`'${name}' state not found`);
        }
        const typedState = namedState as TNamedState;
        return typedState;
      }),
    );
  }
}
