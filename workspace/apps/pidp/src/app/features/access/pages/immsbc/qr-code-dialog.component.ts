import { Component, inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { QRCodeComponent } from 'angularx-qrcode';

@Component({
  selector: 'app-qr-code-dialog',
  template: `
    <h2 mat-dialog-title>Enrolment Link for {{ data.role }}</h2>
    <mat-dialog-content>
      <p>Share this link and/or QR code with {{ data.pharmacyName }} {{ data.role }}(s).<br />This link and QR code are valid for 90 days</p>
      <div class="qr-code-container">
        <qrcode [qrdata]="data.link" [width]="200" [errorCorrectionLevel]="'M'"></qrcode>
      </div>
      <mat-form-field class="w-100">
        <input matInput [value]="data.link" readonly />
        <button mat-icon-button matSuffix (click)="copyLink(data.link)" aria-label="Copy link">
          <mat-icon>content_copy</mat-icon>
        </button>
      </mat-form-field>
    </mat-dialog-content>
  `,
  styles: ['.qr-code-container { display: flex; justify-content: center; margin: 1rem 0; }'],
  standalone: true,
  imports: [MatDialogModule, MatButtonModule, MatIconModule, MatFormFieldModule, MatInputModule, QRCodeComponent]
})
export class QrCodeDialogComponent {
  data = inject<{
    link: string;
    pharmacyName: string;
    role: string;
}>(MAT_DIALOG_DATA);


  public copyLink(link: string): void {
    navigator.clipboard.writeText(link);
  }
}