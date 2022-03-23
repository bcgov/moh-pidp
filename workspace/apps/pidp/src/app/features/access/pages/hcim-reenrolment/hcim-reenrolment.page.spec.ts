import { TestBed } from '@angular/core/testing';

import { HcimReenrolmentPage } from './hcim-reenrolment.page';

describe('HcimReenrolmentPage', () => {
  let component: HcimReenrolmentPage;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [HcimReenrolmentPage],
    });

    component = TestBed.inject(HcimReenrolmentPage);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
