import { TestBed } from '@angular/core/testing';

import { HcimEnrolmentComponent } from './hcim-enrolment.component';

describe('HcimEnrolmentComponent', () => {
  let component: HcimEnrolmentComponent;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [HcimEnrolmentComponent],
    });

    component = TestBed.inject(HcimEnrolmentComponent);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
