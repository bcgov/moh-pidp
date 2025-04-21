import { TestBed } from '@angular/core/testing';

import { HaloResource } from './halo-resource.service';

describe('HaloResource', () => {
  let service: HaloResource;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(HaloResource);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
