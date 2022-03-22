import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NextStepsPage } from './next-steps.page';

describe('NextStepsPage', () => {
  let component: NextStepsPage;
  let fixture: ComponentFixture<NextStepsPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [NextStepsPage],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(NextStepsPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
