import { TestBed } from '@angular/core/testing';

import { provideAutoSpy } from 'jest-auto-spies';
import { PartyActionsResolver } from './party-actions.resolver';
import { PartyResolver } from './party.resolver';
import { LandingActionsResolver } from './landing-actions.resolver';

describe('PartyActionsResolver', () => {
  let resolver: PartyActionsResolver;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        provideAutoSpy(PartyResolver),
        provideAutoSpy(LandingActionsResolver),
      ],
    });
    resolver = TestBed.inject(PartyActionsResolver);
  });

  it('should be created', () => {
    expect(resolver).toBeTruthy();
  });
});
