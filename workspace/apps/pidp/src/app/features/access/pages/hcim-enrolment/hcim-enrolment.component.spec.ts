import { TestBed } from '@angular/core/testing';

import { HcimEnrolmentComponent } from './hcim-enrolment.component';

describe('HcimEnrolmentComponent', () => {
  let component: HcimEnrolmentComponent;

  beforeEach(async () => {
    TestBed.configureTestingModule({
      providers: [HcimEnrolmentComponent],
    });
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
