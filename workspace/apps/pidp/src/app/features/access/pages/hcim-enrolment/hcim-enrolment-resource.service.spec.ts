import { TestBed } from '@angular/core/testing';

import { HcimEnrolmentResource } from './hcim-enrolment-resource.service';

describe('HcimEnrolmentResource', () => {
  let service: HcimEnrolmentResource;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(HcimEnrolmentResource);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
