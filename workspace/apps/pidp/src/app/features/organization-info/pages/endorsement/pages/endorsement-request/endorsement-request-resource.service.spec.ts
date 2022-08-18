import { TestBed } from '@angular/core/testing';

import { EndorsementRequestResource } from './endorsement-request-resource.service';

describe('EndorsementRequestResource', () => {
  let service: EndorsementRequestResource;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(EndorsementRequestResource);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
