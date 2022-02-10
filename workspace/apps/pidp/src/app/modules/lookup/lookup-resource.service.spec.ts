import { TestBed } from '@angular/core/testing';

import { LookupResource } from './lookup-resource.service';

describe('LookupResource', () => {
  let service: LookupResource;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(LookupResource);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
