import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HcimWebEnrolmentComponent } from './hcim-web-enrolment.component';

describe('HcimWebEnrolmentComponent', () => {
  let component: HcimWebEnrolmentComponent;
  let fixture: ComponentFixture<HcimWebEnrolmentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ HcimWebEnrolmentComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(HcimWebEnrolmentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
