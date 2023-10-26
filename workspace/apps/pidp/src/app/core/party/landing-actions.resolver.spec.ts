import { TestBed } from '@angular/core/testing';

import { LandingActionsResolver } from './landing-actions.resolver';

describe('LandingActionsResolver', () => {
  let resolver: LandingActionsResolver;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    resolver = TestBed.inject(LandingActionsResolver);
  });

  it('should be created', () => {
    expect(resolver).toBeTruthy();
  });
});
