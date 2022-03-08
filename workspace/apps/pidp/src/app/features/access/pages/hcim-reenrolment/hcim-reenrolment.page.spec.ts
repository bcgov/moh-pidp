import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HcimReenrolmentPage } from './hcim-reenrolment.page';

describe('HcimReenrolmentPage', () => {
  let component: HcimReenrolmentPage;
  let fixture: ComponentFixture<HcimReenrolmentPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [HcimReenrolmentPage],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(HcimReenrolmentPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
