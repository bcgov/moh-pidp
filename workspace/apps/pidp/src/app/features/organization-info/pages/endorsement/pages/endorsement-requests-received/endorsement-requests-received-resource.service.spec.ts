import { TestBed } from '@angular/core/testing';

import { EndorsementRequestsReceivedResource } from './endorsement-requests-received-resource.service';

describe('EndorsementRequestsReceivedResourceService', () => {
  let service: EndorsementRequestsReceivedResource;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(EndorsementRequestsReceivedResource);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
