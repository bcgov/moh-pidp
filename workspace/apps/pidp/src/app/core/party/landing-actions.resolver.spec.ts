import { TestBed } from '@angular/core/testing';

import { landingActionsResolver } from './landing-actions.resolver';
import { provideAutoSpy } from 'jest-auto-spies';
import { PartyService } from './party.service';
import { Router } from '@angular/router';
import { EndorsementsResource } from '@app/features/organization-info/pages/endorsements/endorsements-resource.service';

describe('LandingActionsResolver', () => {
  let resolver: landingActionsResolver;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        provideAutoSpy(PartyService),
        provideAutoSpy(Router),
        provideAutoSpy(EndorsementsResource),
      ],
    });
    resolver = TestBed.inject(landingActionsResolver);
  });

  it('should be created', () => {
    expect(resolver).toBeTruthy();
  });
});
