import { TestBed } from '@angular/core/testing';

import { EndorsementResource } from './endorsement-resource.service';

describe('EndorsementResource', () => {
  let service: EndorsementResource;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(EndorsementResource);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
