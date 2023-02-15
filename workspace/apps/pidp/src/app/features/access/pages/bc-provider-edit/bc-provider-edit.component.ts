import { Component, TemplateRef, ViewChild } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute } from '@angular/router';

import { Observable, catchError, of, tap } from 'rxjs';

import { faCircleCheck, faXmark } from '@fortawesome/free-solid-svg-icons';
import { NavigationService } from '@pidp/presentation';

import { AbstractFormPage } from '@app/core/classes/abstract-form-page.class';
import { PartyService } from '@app/core/party/party.service';
import { FormUtilsService } from '@app/core/services/form-utils.service';

import { BcProviderEditFormState } from './bc-provider-edit-form-state';
import {
  BcProviderChangePasswordRequest,
  BcProviderEditResource,
} from './bc-provider-edit.resource';

export interface BcProviderEditInitialStateModel {
  username: string;
}

@Component({
  selector: 'app-bc-provider-edit',
  templateUrl: './bc-provider-edit.component.html',
  styleUrls: ['./bc-provider-edit.component.scss'],
})
export class BcProviderEditComponent extends AbstractFormPage<BcProviderEditFormState> {
  public faCircleCheck = faCircleCheck;
  public faXmark = faXmark;
  public formState: BcProviderEditFormState;
  public showErrorCard = false;
  public errorCardText = '';
  public showMessageCard = false;
  public messageCardText = '';

  @ViewChild('successDialog')
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  public successDialogTemplate!: TemplateRef<any>;

  public get isResetButtonEnabled(): boolean {
    return this.formState.form.valid;
  }
  public username: string;

  public constructor(
    dialog: MatDialog,
    formUtilsService: FormUtilsService,
    fb: FormBuilder,
    private navigationService: NavigationService,
    private snackBar: MatSnackBar,
    private partyService: PartyService,
    private bcProviderEditResource: BcProviderEditResource,
    route: ActivatedRoute
  ) {
    super(dialog, formUtilsService);
    this.formState = new BcProviderEditFormState(fb);

    const model = route.snapshot.data
      .bcProviderEditData as BcProviderEditInitialStateModel;
    this.username = model?.username ?? '';
  }

  public onBack(): void {
    this.navigationService.navigateToRoot();
  }
  public onGeneratePasswordCheckChange(): void {
    this.snackBar.open('Not yet implemented', 'OK');
  }

  public hasPasswordRuleError(): boolean {
    return this.formState.newPassword.hasError('invalidRequirements');
  }

  public onSuccessDialogClose(): void {
    this.dialog.closeAll();
    this.navigationService.navigateToRoot();
  }

  protected performSubmission(): Observable<boolean> {
    const data: BcProviderChangePasswordRequest = {
      partyId: this.partyService.partyId,
      username: this.username,
      newPassword: this.formState.newPassword.value,
    };
    return this.bcProviderEditResource.changePassword(data).pipe(
      tap((_) => {
        this.setError('');
        this.snackBar.open(
          'No AD account was actually created. This is a work in progress.',
          'OK'
        );
        this.showSuccessDialog();
      }),
      catchError((response) => {
        console.log('error', response);
        let message = 'An error occurred.';
        if (response?.error?.errors) {
          const errors = response.error.errors;
          if (errors.Password?.length > 0) {
            message = errors.Password[0];
          }
        }
        this.setError(message);
        this.setMessage('');
        return of(false);
      })
    );
  }

  private setError(message: string): void {
    this.showErrorCard = !!message;
    this.errorCardText = message;
  }

  private setMessage(message: string): void {
    this.showMessageCard = !!message;
    this.messageCardText = message;
  }

  private showSuccessDialog(): void {
    const config: MatDialogConfig = {
      disableClose: true,
    };
    this.dialog.open(this.successDialogTemplate, config);
  }
}
