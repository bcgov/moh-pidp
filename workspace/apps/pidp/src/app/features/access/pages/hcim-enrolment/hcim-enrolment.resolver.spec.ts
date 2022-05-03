import { TestBed } from '@angular/core/testing';

import { HcimEnrolmentResolver } from './hcim-enrolment.resolver';

describe('HcimEnrolmentResolver', () => {
  let resolver: HcimEnrolmentResolver;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    resolver = TestBed.inject(HcimEnrolmentResolver);
  });

  it('should be created', () => {
    expect(resolver).toBeTruthy();
  });
});
