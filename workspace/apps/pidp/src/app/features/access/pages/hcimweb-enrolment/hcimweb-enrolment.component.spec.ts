import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HcimwebEnrolmentComponent } from './hcimweb-enrolment.component';

describe('HcimwebEnrolmentComponent', () => {
  let component: HcimwebEnrolmentComponent;
  let fixture: ComponentFixture<HcimwebEnrolmentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ HcimwebEnrolmentComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(HcimwebEnrolmentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
