import { CommonModule } from '@angular/common';
import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatNativeDateModule } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import {
  MAT_DIALOG_DATA,
  MatDialogModule,
  MatDialogRef,
} from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { MatSelectModule } from '@angular/material/select';

import { IStaff, PharmacyRole } from './pharmacy-staff.model';
import { PharmacyResource } from './pharmacy-resource.service';

export interface EditStaffDialogData {
  staff: IStaff;
  pharmacyId: number;
}

@Component({
  selector: 'app-edit-staff-dialog',
  template: `
    <h2 mat-dialog-title>Edit Staff: </h2>
    <mat-dialog-content>
      <form [formGroup]="form">
        <mat-form-field class="w-100">
          <mat-label>Role</mat-label>
          <mat-select formControlName="role">
            <mat-option [value]="PharmacyRole.Clinician">Clinician</mat-option>
            <mat-option [value]="PharmacyRole.Clerk">Clerk</mat-option>
            <mat-option [value]="PharmacyRole.Admin">Administrator</mat-option>
            <mat-option [value]="PharmacyRole.Unknown">None</mat-option>
          </mat-select>
        </mat-form-field>

        <mat-form-field class="w-100">
          <mat-label>Effective Start Date</mat-label>
          <input
            matInput
            [matDatepicker]="startPicker"
            formControlName="effectiveStartDate"
          />
          <mat-datepicker-toggle
            matSuffix
            [for]="startPicker"
          ></mat-datepicker-toggle>
          <mat-datepicker #startPicker></mat-datepicker>
        </mat-form-field>

        <mat-form-field class="w-100">
          <mat-label>Effective End Date</mat-label>
          <input
            matInput
            [matDatepicker]="endPicker"
            formControlName="effectiveEndDate"
          />
          <button
            mat-icon-button
            matSuffix
            (click)="extendDate()"
            type="button"
            aria-label="Set to next August 1st"
          >
            <mat-icon>update</mat-icon>
          </button>
          <mat-datepicker-toggle
            matSuffix
            [for]="endPicker"
          ></mat-datepicker-toggle>
          <mat-datepicker #endPicker></mat-datepicker>
        </mat-form-field>
      </form>
    </mat-dialog-content>
    <mat-dialog-actions align="end">
      <button mat-button (click)="onCancel()">Cancel</button>
      <button
        mat-flat-button
        color="primary"
        (click)="onSave()"
        [disabled]="form.invalid || !form.dirty"
      >
        Save
      </button>
    </mat-dialog-actions>
  `,
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatButtonModule,
    MatFormFieldModule,
    MatIconModule,
    MatInputModule,
    MatSelectModule,
    MatDatepickerModule,
    MatNativeDateModule,
  ],
})
export class EditStaffDialogComponent implements OnInit {
  public form: FormGroup;
  public PharmacyRole = PharmacyRole;

  public constructor(
    private readonly fb: FormBuilder,
    private readonly dialogRef: MatDialogRef<EditStaffDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: EditStaffDialogData,
    private readonly pharmacyResource: PharmacyResource
  ) {
    this.form = this.fb.group({
      role: [this.data.staff.role],
      effectiveStartDate: [
        this.data.staff.effectiveStartDate
          ? new Date(this.data.staff.effectiveStartDate)
          : null,
      ],
      effectiveEndDate: [
        this.data.staff.effectiveEndDate
          ? new Date(this.data.staff.effectiveEndDate)
          : null,
      ],
    });
  }

  public ngOnInit(): void {
    this.form.get('effectiveStartDate')?.valueChanges.subscribe((startDate) => {
      if (!startDate) return;

      const endDateControl = this.form.get('effectiveEndDate');
      const currentEndDate = endDateControl?.value;
      const newStartDate = new Date(startDate);

      if (!currentEndDate || currentEndDate < newStartDate) {
        const newEndDate = new Date(newStartDate);
        newEndDate.setMonth(newEndDate.getMonth() + 3);
        endDateControl?.setValue(newEndDate);
      }
    });
  }

  public extendDate(): void {
    const today = new Date();
    const currentYear = today.getFullYear();
    const nextAugustFirst = new Date(currentYear, 7, 1); // Month is 0-indexed

    if (today >= nextAugustFirst) {
      nextAugustFirst.setFullYear(currentYear + 1);
    }

    this.form.get('effectiveEndDate')?.setValue(nextAugustFirst);
    this.form.markAsDirty();
  }

  public onCancel(): void {
    this.dialogRef.close();
  }

  public onSave(): void {
    if (this.form.valid && this.form.dirty) {
      const payload = this.form.getRawValue();

      // Format dates to YYYY-MM-DD string if they are not null
      payload.effectiveStartDate = payload.effectiveStartDate
        ? new Date(payload.effectiveStartDate).toISOString().split('T')[0]
        : null;
      payload.effectiveEndDate = payload.effectiveEndDate
        ? new Date(payload.effectiveEndDate).toISOString().split('T')[0]
        : null;

      this.pharmacyResource
        .updateStaff(this.data.pharmacyId, this.data.staff.partyId, payload)
        .subscribe(() => this.dialogRef.close(true));
    }
  }
}