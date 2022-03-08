import { TestBed } from '@angular/core/testing';

import { HcimReenrolmentResource } from './hcim-reenrolment-resource.service';

describe('HcimWebEnrolmentResource', () => {
  let service: HcimReenrolmentResource;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(HcimReenrolmentResource);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
