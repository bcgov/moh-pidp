import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FeedbackButtonComponent } from './feedback-button.component';
describe('FeedbackButtonComponent', () => {
  let component: FeedbackButtonComponent;
  let fixture: ComponentFixture<FeedbackButtonComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FeedbackButtonComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(FeedbackButtonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
