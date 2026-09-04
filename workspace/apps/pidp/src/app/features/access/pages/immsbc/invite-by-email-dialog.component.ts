import { Component, inject } from '@angular/core';
import { FormBuilder, Validators, ReactiveFormsModule, AbstractControl, ValidationErrors } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-invite-by-email-dialog',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
  ],
  template: `
    <h2 mat-dialog-title>Invite by Email</h2>
    <mat-dialog-content>
      <p>Enter a comma or semicolon separated list of emails to send invitations to {{ data.pharmacyName }}.</p>
      
      <form [formGroup]="form">
        <mat-form-field appearance="outline" class="w-100 mt-3">
          <mat-label>Email Addresses</mat-label>
          <textarea
            matInput
            formControlName="emails"
            placeholder="e.g. staff1@example.com; staff2@example.com"
            rows="4"
          ></textarea>
          @if (form.get('emails')?.hasError('required') && form.get('emails')?.touched) {
            <mat-error>Email addresses are required.</mat-error>
          }
          @if (form.get('emails')?.hasError('invalidEmails') && form.get('emails')?.touched) {
            <mat-error>One or more emails are invalid.</mat-error>
          }
        </mat-form-field>
      </form>
    </mat-dialog-content>
    <mat-dialog-actions align="end">
      <button mat-button mat-dialog-close>Cancel</button>
      <button
        mat-flat-button
        color="primary"
        [disabled]="form.invalid || !form.dirty"
        (click)="onInvite()"
      >
        Invite
      </button>
    </mat-dialog-actions>
  `,
  styles: [`
    .w-100 { width: 100%; }
    .mt-3 { margin-top: 1rem; }
  `]
})
export class InviteByEmailDialogComponent {
  public readonly data = inject<{
    pharmacyName: string;
    role: string;
  }>(MAT_DIALOG_DATA);
  
  private readonly dialogRef = inject(MatDialogRef<InviteByEmailDialogComponent>);
  private readonly fb = inject(FormBuilder);

  public readonly form = this.fb.group({
    emails: ['', [Validators.required, this.emailsValidator.bind(this)]]
  });

  private emailsValidator(control: AbstractControl): ValidationErrors | null {
    if (!control.value) {
      return null;
    }
    
    const emails = this.parseEmails(control.value);
    if (emails.length === 0) {
      return { invalidEmails: true };
    }

    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    const allValid = emails.every((email: string) => emailRegex.test(email));
    
    return allValid ? null : { invalidEmails: true };
  }

  private parseEmails(input: string): string[] {
    return input
      .split(/[;,]/)
      .map(e => e.trim())
      .filter(e => e.length > 0);
  }

  public onInvite(): void {
    if (this.form.valid) {
      const emailList = this.parseEmails(this.form.value.emails || '');
      this.dialogRef.close(emailList);
    }
  }
}
