import { Injectable } from '@angular/core';

import { BehaviorSubject, Observable } from 'rxjs';

import { Party } from '@bcgov/shared/data-access';

@Injectable({
  providedIn: 'root',
})
export class PartyService {
  private _party$: BehaviorSubject<Party | null>;

  public constructor() {
    this._party$ = new BehaviorSubject<Party | null>(null);
  }

  public set party(party: Party | null) {
    this._party$.next(party);
  }

  public get party(): Party | null {
    return this._party$.value;
  }

  public party$(): Observable<Party | null> {
    return this._party$.asObservable();
  }
}
