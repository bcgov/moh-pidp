import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';

import { Observable, catchError, tap } from 'rxjs';

import { faUser } from '@fortawesome/free-solid-svg-icons';
import { NavigationService } from '@pidp/presentation';

import { AbstractFormPage } from '@app/core/classes/abstract-form-page.class';
import { PartyService } from '@app/core/party/party.service';
import { FormUtilsService } from '@app/core/services/form-utils.service';

import { BcProviderApplicationFormState } from './bc-provider-application-form-state';
import {
  BcProviderApplicationRequest,
  BcProviderApplicationResource,
} from './bc-provider-application-resource.service';

@Component({
  selector: 'app-bc-provider-application',
  templateUrl: './bc-provider-application.component.html',
  styleUrls: ['./bc-provider-application.component.scss'],
})
export class BcProviderApplicationComponent extends AbstractFormPage<BcProviderApplicationFormState> {
  public faUser = faUser;
  public formState: BcProviderApplicationFormState;
  public showErrorCard = false;
  public errorCardText = '';
  public showMessageCard = false;
  public messageCardText = '';

  public get isEnrolButtonEnabled(): boolean {
    return this.formState.form.valid;
  }

  public constructor(
    dialog: MatDialog,
    formUtilsService: FormUtilsService,
    fb: FormBuilder,
    private navigationService: NavigationService,
    private snackBar: MatSnackBar,
    private partyService: PartyService,
    private bcProviderApplicationResource: BcProviderApplicationResource
  ) {
    super(dialog, formUtilsService);
    this.formState = new BcProviderApplicationFormState(fb);
  }

  public onBack(): void {
    this.navigationService.navigateToRoot();
  }
  public onGeneratePasswordCheckChange(): void {
    this.snackBar.open('Not yet implemented', 'OK');
  }
  public hasPasswordRuleError(): boolean {
    const hasMinlengthError = this.formState.password.hasError('minlength');
    const hasMaxlengthError = this.formState.password.hasError('maxlength');
    return hasMinlengthError || hasMaxlengthError;
  }

  protected performSubmission(): Observable<string | null> {
    const data: BcProviderApplicationRequest = {
      partyId: this.partyService.partyId,
      username: this.formState.username.value,
      password: this.formState.password.value,
    };
    return this.bcProviderApplicationResource
      .createBcProviderAccount(data)
      .pipe(
        tap((_) => {
          this.setError('');
          this.setMessage(
            'The back end was called but no AD account was actually created. This is a work in progress.'
          );
          this.snackBar.open(
            'No AD account was actually created. This is a work in progress.',
            'OK'
          );
        }),
        catchError((response) => {
          let message = 'An error occurred.';
          if (response?.error?.errors) {
            const errors = response.error.errors;
            if (errors.Password?.length > 0) {
              message = errors.Password[0];
            }
          }
          this.setError(message);
          this.setMessage('');
          return '';
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
}
