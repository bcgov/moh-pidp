import { TestBed } from '@angular/core/testing';

import { EndorsementRequestResolver } from './endorsement-request.resolver';

describe('EndorsementRequestResolver', () => {
  let resolver: EndorsementRequestResolver;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    resolver = TestBed.inject(EndorsementRequestResolver);
  });

  it('should be created', () => {
    expect(resolver).toBeTruthy();
  });
});
