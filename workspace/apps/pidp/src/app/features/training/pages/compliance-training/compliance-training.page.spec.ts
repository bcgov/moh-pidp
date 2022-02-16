import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ComplianceTrainingPage } from './compliance-training.page';

describe('ComplianceTrainingPage', () => {
  let component: ComplianceTrainingPage;
  let fixture: ComponentFixture<ComplianceTrainingPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ComplianceTrainingPage],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ComplianceTrainingPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
