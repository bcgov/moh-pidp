import { TestBed } from '@angular/core/testing';

import { EndorsementResolver } from './endorsement.resolver';

describe('EndorsementResolver', () => {
  let resolver: EndorsementResolver;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    resolver = TestBed.inject(EndorsementResolver);
  });

  it('should be created', () => {
    expect(resolver).toBeTruthy();
  });
});
