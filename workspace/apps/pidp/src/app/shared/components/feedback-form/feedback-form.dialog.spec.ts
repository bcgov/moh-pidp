import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FeedbackFormDialogComponent } from './feedback-form.dialog';
import { FormUtilsService } from '@app/core/services/form-utils.service';
import { MatDialogRef } from '@angular/material/dialog';
import { provideAutoSpy } from 'jest-auto-spies';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

describe('FeedbackFormDialogComponent', () => {
  let component: FeedbackFormDialogComponent;
  let fixture: ComponentFixture<FeedbackFormDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FeedbackFormDialogComponent, BrowserAnimationsModule],
      providers: [
        provideAutoSpy(FormUtilsService),
        provideAutoSpy(MatDialogRef)
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(FeedbackFormDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
