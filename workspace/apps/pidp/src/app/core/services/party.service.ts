import { Injectable } from '@angular/core';

import { BehaviorSubject, Observable } from 'rxjs';

import { BcscUser, Party } from '@bcgov/shared/data-access';

import { PortalSection } from '@app/features/portal/portal.component';

import { DemoService } from './demo.service';

@Injectable({
  providedIn: 'root',
})
export class PartyService {
  private _party$: BehaviorSubject<Party | null>;
  private _acceptedCollectionNotice: boolean;

  public constructor(private demoService: DemoService) {
    this._party$ = new BehaviorSubject<Party | null>(null);
    this._acceptedCollectionNotice = true;
  }

  public get state(): Record<string, PortalSection[]> {
    return this.demoService.state;
  }

  public get user(): BcscUser {
    return {
      hpdid: '00000000-0000-0000-0000-000000000000',
      firstName: 'Lucy',
      lastName: 'Pultz',
    };
  }

  public set acceptedCollectionNotice(hasAccepted: boolean) {
    this._acceptedCollectionNotice = hasAccepted;
  }

  public get acceptedCollectionNotice(): boolean {
    return this._acceptedCollectionNotice;
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
