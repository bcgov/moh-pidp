import { TestBed } from '@angular/core/testing';

import { PrescriptionRefillEformsResource } from './prescription-refill-eforms-resource.service';

describe('PrescriptionRefillEformsResource', () => {
  let service: PrescriptionRefillEformsResource;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(PrescriptionRefillEformsResource);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
