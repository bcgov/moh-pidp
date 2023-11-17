import { TestBed } from '@angular/core/testing';

import { provideAutoSpy } from 'jest-auto-spies';
import { partyActionsResolver } from './party-actions.resolver';
import { partyResolver } from './party.resolver';
import { landingActionsResolver } from './landing-actions.resolver';

describe('PartyActionsResolver', () => {
  let resolver: partyActionsResolver;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        provideAutoSpy(partyResolver),
        provideAutoSpy(landingActionsResolver),
      ],
    });
    resolver = TestBed.inject(partyActionsResolver);
  });

  it('should be created', () => {
    expect(resolver).toBeTruthy();
  });
});
