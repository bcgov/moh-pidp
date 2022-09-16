import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';

import { EMPTY } from 'rxjs';

import { NoContent } from '@bcgov/shared/data-access';

import { AbstractFormPage } from '@app/core/classes/abstract-form-page.class';
import { FormUtilsService } from '@app/core/services/form-utils.service';

import { MsTeamsFormState } from './ms-teams-form-state';

@Component({
  selector: 'app-clinic-member-form',
  template: `
    <div class="clinic-member-form">
      <ng-container [formGroup]="form">
        <mat-icon class="material-icons-sharp">account_circle</mat-icon>
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
          <mat-error>Required</mat-error>
        </mat-form-field>
        <mat-form-field class="w-100 form-field" appearance="fill">
          <input
            matInput
            placeholder="Phone Number"
            formControlName="phone"
            mask="(000) 000-0000"
            [showMaskTyped]="false" />
          <mat-error>Required</mat-error>
        </mat-form-field>
      </ng-container>
    </div>
  `,
  styles: [
    `
      .clinic-member-form {
        display: flex !important;
      }

      .form-field {
        margin: 5px;
      }

      mat-icon {
        align-self: center;
      }
    `,
  ],
})
export class ClinicMemberFormComponent
  extends AbstractFormPage
  implements OnInit
{
  @Input() public form: FormGroup;
  @Input() public index!: number;

  public formState: MsTeamsFormState;

  public constructor(
    protected dialog: MatDialog,
    protected formUtilsService: FormUtilsService,
    fb: FormBuilder
  ) {
    super(dialog, formUtilsService);
    this.formState = new MsTeamsFormState(fb, formUtilsService);
    this.form = this.formState.buildClinicMemberForm();
  }

  public ngOnInit(): void {}

  protected performSubmission(): NoContent {
    return EMPTY;
  }
}
