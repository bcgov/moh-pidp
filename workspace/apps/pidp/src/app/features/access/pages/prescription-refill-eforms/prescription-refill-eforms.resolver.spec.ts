import { TestBed } from '@angular/core/testing';

import { PrescriptionRefillEformsResolver } from './prescription-refill-eforms.resolver';

describe('PrescriptionRefillEformsResolver', () => {
  let resolver: PrescriptionRefillEformsResolver;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    resolver = TestBed.inject(PrescriptionRefillEformsResolver);
  });

  it('should be created', () => {
    expect(resolver).toBeTruthy();
  });
});
