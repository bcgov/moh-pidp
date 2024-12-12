import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FeedbackFormDialogComponent } from './feedback-form.dialog';
import { FormUtilsService } from '@app/core/services/form-utils.service';
import { MatDialogRef } from '@angular/material/dialog';
import { provideAutoSpy, Spy } from 'jest-auto-spies';
import { FeedbackSendComponent } from '../success-dialog/components/feedback-send.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

describe('FeedbackFormDialogComponent', () => {
  let component: FeedbackFormDialogComponent;
  let fixture: ComponentFixture<FeedbackFormDialogComponent>;
  let formUtilsServiceSpy: Spy<FormUtilsService>;
  let matDialogRefSpy: Spy<MatDialogRef<FeedbackSendComponent>>;

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
    formUtilsServiceSpy = TestBed.inject<any>(FormUtilsService);
    matDialogRefSpy = TestBed.inject<any>(MatDialogRef);
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
