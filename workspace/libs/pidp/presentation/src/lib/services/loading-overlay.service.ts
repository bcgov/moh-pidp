import { NgIf } from '@angular/common';
import { Component, Inject, Injectable } from '@angular/core';
import {
  MAT_DIALOG_DATA,
  MatDialog,
  MatDialogConfig,
  MatDialogRef,
} from '@angular/material/dialog';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

export const LOADING_OVERLAY_DEFAULT_MESSAGE =
  'Your request is being processed';
/**
 * Provides methods for displaying and hiding an overlay with a spinner. The overlay blocks interaction
 * with the screen.
 */
@Injectable({ providedIn: 'root' })
export class LoadingOverlayService {
  constructor(private dialog: MatDialog) {}

  public open(message?: string): void {
    // Uses mat-dialog to display the overlay with minimal custom UI code and CSS.

    const data: PidpLoadingDialogData = {
      message: message,
    };

    const config: MatDialogConfig = {
      // Prevent the user from closing the dialog
      disableClose: true,
      data: data,
    };

    this.dialog.open(PidpLoadingDialogComponent, config);
  }
  public close(): void {
    this.dialog.closeAll();
  }
}
interface PidpLoadingDialogData {
  message?: string;
}
/**
 * Dialog that displays the spinner.
 */
@Component({
    // Putting template and css inline for simplicity.
    template: `<div class="loader-container">
    <div class="spinner">
      <mat-spinner color="primary" mode="indeterminate"></mat-spinner>
    </div>
    <div *ngIf="data.message" class="message">
      {{ data.message }}
    </div>
  </div>`,
    styles: [
        `
      .loader-container {
        padding: 24px;
      }
      .spinner {
        display: flex;
        justify-content: center;
      }
      .message {
        margin-top: 1rem;
      }
    `,
    ],
    imports: [MatProgressSpinnerModule, NgIf]
})
export class PidpLoadingDialogComponent {
  public constructor(
    public dialogRef: MatDialogRef<PidpLoadingDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: PidpLoadingDialogData,
  ) {}
}
