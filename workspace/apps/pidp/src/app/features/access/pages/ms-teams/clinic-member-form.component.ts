import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';

@Component({
  selector: 'app-clinic-member-form',
  template: `
    <div class="clinic-member-form">
      <ng-container [formGroup]="form">
        <mat-icon>account_circle</mat-icon>
        <mat-form-field class="w-100 form-field" appearance="fill">
          <input matInput placeholder="Name" formControlName="name" />
          <mat-error>Required</mat-error>
        </mat-form-field>
        <mat-form-field class="w-100 form-field" appearance="fill">
          <input matInput placeholder="Job Title" formControlName="jobTitle" />
          <mat-error>Required</mat-error>
        </mat-form-field>
        <mat-form-field class="w-100 form-field" appearance="fill">
          <input matInput placeholder="Email" formControlName="email" />
          <mat-error *ngIf="email.hasError('required')">Required</mat-error>
          <mat-error *ngIf="email.hasError('email')">
            Must be a valid email address
          </mat-error>
        </mat-form-field>
        <mat-form-field class="w-100 form-field" appearance="fill">
          <input
            matInput
            placeholder="Phone Number"
            formControlName="phone"
            mask="(000) 000-0000"
            [showMaskTyped]="false" />
          <mat-error *ngIf="phone.hasError('required')">Required</mat-error>
          <mat-error *ngIf="phone.hasError('phone')">
            Must be a valid phone number
          </mat-error>
        </mat-form-field>
        <button
          *ngIf="length > 1 && index !== 0"
          mat-icon-button
          type="button"
          (click)="removeForm(index)">
          <mat-icon>close</mat-icon>
        </button>
      </ng-container>
    </div>
  `,
  styles: [
    `
      .clinic-member-form {
        display: flex !important;
        width: max-content;
      }

      .form-field {
        margin: 5px;
      }

      mat-icon,
      .mat-icon-button {
        align-self: center;
      }
    `,
  ],
})
export class ClinicMemberFormComponent {
  @Input() public form!: FormGroup;
  @Input() public index!: number;
  @Input() public length!: number;
  @Output() public remove: EventEmitter<number>;

  public constructor() {
    this.remove = new EventEmitter<number>();
  }

  public get phone(): FormControl {
    return this.form.get('phone') as FormControl;
  }

  public get email(): FormControl {
    return this.form.get('email') as FormControl;
  }

  public removeForm(index: number): void {
    this.remove.emit(index);
  }
}
