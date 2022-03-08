import { TestBed } from '@angular/core/testing';

import { HcimWebEnrolmentResource } from './hcim-web-enrolment-resource.service';

describe('HcimWebEnrolmentResource', () => {
  let service: HcimWebEnrolmentResource;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(HcimWebEnrolmentResource);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
