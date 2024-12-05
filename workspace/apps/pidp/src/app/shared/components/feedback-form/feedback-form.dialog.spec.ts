import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FeedbackFormDialogComponent } from './feedback-form.dialog';
describe('FeedbackFormDialogComponent', () => {
  let component: FeedbackFormDialogComponent;
  let fixture: ComponentFixture<FeedbackFormDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FeedbackFormDialogComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(FeedbackFormDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
