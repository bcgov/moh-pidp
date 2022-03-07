import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HcimWebEnrolmentPage } from './hcim-web-enrolment.page';

describe('HcimWebEnrolmentPage', () => {
  let component: HcimWebEnrolmentPage;
  let fixture: ComponentFixture<HcimWebEnrolmentPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [HcimWebEnrolmentPage],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(HcimWebEnrolmentPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
