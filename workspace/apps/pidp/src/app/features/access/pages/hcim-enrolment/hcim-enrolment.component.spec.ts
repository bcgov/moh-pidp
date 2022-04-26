import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HcimEnrolmentComponent } from './hcim-enrolment.component';

describe('HcimEnrolmentComponent', () => {
  let component: HcimEnrolmentComponent;
  let fixture: ComponentFixture<HcimEnrolmentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [HcimEnrolmentComponent],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(HcimEnrolmentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
