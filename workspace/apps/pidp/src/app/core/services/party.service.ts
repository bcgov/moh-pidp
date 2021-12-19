import { Injectable } from '@angular/core';

import { BehaviorSubject, Observable } from 'rxjs';

import { DateTime } from 'luxon';

import { Address, BcscUser, Party } from '@bcgov/shared/data-access';

import { PortalSection } from '@app/features/portal/portal.component';

import { DemoService } from './demo.service';

@Injectable({
  providedIn: 'root',
})
export class PartyService {
  private _party$: BehaviorSubject<Party | null>;

  public constructor(private demoService: DemoService) {
    this._party$ = new BehaviorSubject<Party | null>(null);
  }

  public get state(): Record<string, PortalSection[]> {
    return this.demoService.state;
  }

  public get user(): BcscUser {
    return {
      hpdid: '00000000-0000-0000-0000-000000000000',
      firstName: 'Lucy',
      lastName: 'Pultz',
      dateOfBirth: DateTime.now().minus({ years: 38 }).toISODate(),
      verifiedAddress: new Address(
        'CA',
        'BC',
        '524 Coral Dr.',
        'Victoria',
        'V9S 2L7'
      ),
    };
  }

  public get acceptedCollectionNotice(): boolean {
    return false;
  }

  public get completedProfile(): boolean {
    return true;
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
