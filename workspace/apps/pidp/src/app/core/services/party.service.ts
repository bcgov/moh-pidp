import { Injectable } from '@angular/core';

import { BehaviorSubject, Observable } from 'rxjs';

import { Party } from '@bcgov/shared/data-access';

import { BcscUser } from '@app/features/auth/models/bcsc-user.model';
import { PortalSection } from '@app/features/portal/portal.component';

import { DemoService } from './demo.service';

// TODO create custom initializer that inits keycloak then gets partyId and stores in this service for easy replacement by state management
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

  // TODO temporary state for demos
  public get state(): Record<string, PortalSection[]> {
    return this.demoService.state;
  }

  public get user(): BcscUser {
    return {
      hpdid: '00000000-0000-0000-0000-000000000000',
      userId: '00000000-0000-0000-0000-000000000000',
      firstName: 'Lucy',
      lastName: 'Pultz',
      birthdate: '2021-03-17',
    };
  }

  // TODO temporary state modifier for demos
  public updateState(sectionType: string): void {
    this.demoService.updateState(sectionType);
  }

  public set acceptedCollectionNotice(hasAccepted: boolean) {
    this._acceptedCollectionNotice = hasAccepted;
  }

  public get acceptedCollectionNotice(): boolean {
    return this._acceptedCollectionNotice;
  }

  public get completedProfile(): boolean {
    return this.demoService.profileComplete;
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
