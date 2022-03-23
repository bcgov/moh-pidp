import { TestBed } from '@angular/core/testing';

import { PartyResolver } from './party.resolver';

describe('PartyResolver', () => {
  let resolver: PartyResolver;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    resolver = TestBed.inject(PartyResolver);
  });

  it('should be created', () => {
    expect(resolver).toBeTruthy();
  });
});
