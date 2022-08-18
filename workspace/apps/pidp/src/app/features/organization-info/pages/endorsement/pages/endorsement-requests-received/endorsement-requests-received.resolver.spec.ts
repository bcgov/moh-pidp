import { TestBed } from '@angular/core/testing';

import { EndorsementRequestsReceivedResolver } from './endorsement-requests-received.resolver';

describe('EndorsementRequestsReceivedResolver', () => {
  let resolver: EndorsementRequestsReceivedResolver;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    resolver = TestBed.inject(EndorsementRequestsReceivedResolver);
  });

  it('should be created', () => {
    expect(resolver).toBeTruthy();
  });
});
