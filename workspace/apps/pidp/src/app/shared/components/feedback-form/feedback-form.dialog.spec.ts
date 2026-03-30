import { ComponentFixture, TestBed } from '@angular/core/testing';
import { MatDialogRef } from '@angular/material/dialog';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { provideAutoSpy } from 'jest-auto-spies';

import { PartyService } from '@app/core/party/party.service';
import { FormUtilsService } from '@app/core/services/form-utils.service';
import { LoggerService } from '@app/core/services/logger.service';

import { FeedbackFormDialogResource } from './feedback-form-dialog-resource.service';
import { FeedbackFormDialogComponent } from './feedback-form.dialog';

describe('FeedbackFormDialogComponent', () => {
  let component: FeedbackFormDialogComponent;
  let fixture: ComponentFixture<FeedbackFormDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FeedbackFormDialogComponent, BrowserAnimationsModule],
      providers: [
        provideAutoSpy(FormUtilsService),
        provideAutoSpy(MatDialogRef),
        provideAutoSpy(FeedbackFormDialogResource),
        provideAutoSpy(LoggerService),
        provideAutoSpy(PartyService),
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(FeedbackFormDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
