import { TestBed } from '@angular/core/testing';

import { AccessRequestResource } from './access-request-resource.service';

describe('AccessRequestResource', () => {
  let service: AccessRequestResource;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AccessRequestResource);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
