import { TestBed } from '@angular/core/testing';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';

import { provideAutoSpy } from 'jest-auto-spies';

import { SuccessDialogComponent } from './success-dialog.component';

describe('SuccessDialogComponent', () => {
  let component: SuccessDialogComponent;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [MatDialogModule],
      providers: [SuccessDialogComponent, provideAutoSpy(MatDialog)],
    }).compileComponents();

    component = TestBed.inject(SuccessDialogComponent);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
