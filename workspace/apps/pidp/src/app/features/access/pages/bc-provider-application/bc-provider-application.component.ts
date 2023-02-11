import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute } from '@angular/router';

import { Observable, catchError, tap } from 'rxjs';

import { faUser } from '@fortawesome/free-solid-svg-icons';
import { NavigationService } from '@pidp/presentation';

import { AbstractFormPage } from '@app/core/classes/abstract-form-page.class';
import { PartyService } from '@app/core/party/party.service';
import { FormUtilsService } from '@app/core/services/form-utils.service';
import { LoggerService } from '@app/core/services/logger.service';
import { StatusCode } from '@app/features/portal/enums/status-code.enum';

import { BcProviderApplicationFormState } from './bc-provider-application-form-state';
import { BcProviderApplicationResource } from './bc-provider-application-resource.service';

@Component({
  selector: 'app-bc-provider-application',
  templateUrl: './bc-provider-application.component.html',
  styleUrls: ['./bc-provider-application.component.scss'],
})
export class BcProviderApplicationComponent
  extends AbstractFormPage<BcProviderApplicationFormState>
  implements OnInit
{
  public faUser = faUser;
  public formState: BcProviderApplicationFormState;
  public showErrorCard = false;
  public errorCardText = '';
  public showMessageCard = false;
  public messageCardText = '';
  public completed: boolean | null;
  public password = '';

  public get isEnrolButtonEnabled(): boolean {
    return this.formState.form.valid;
  }

  public constructor(
    private route: ActivatedRoute,
    dialog: MatDialog,
    formUtilsService: FormUtilsService,
    fb: FormBuilder,
    private navigationService: NavigationService,
    private snackBar: MatSnackBar,
    private partyService: PartyService,
    private resource: BcProviderApplicationResource,
    private logger: LoggerService
  ) {
    super(dialog, formUtilsService);
    this.formState = new BcProviderApplicationFormState(fb);
    const routeData = this.route.snapshot.data;
    this.completed =
      routeData.bcProviderApplicationStatusCode == StatusCode.COMPLETED;
  }

  public onBack(): void {
    this.navigationService.navigateToRoot();
  }

  public hasPasswordRuleError(): boolean {
    return this.formState.password.hasError('invalidRequirements');
  }

  public ngOnInit(): void {
    const partyId = this.partyService.partyId;

    if (!partyId) {
      this.logger.error('No party ID was provided');
      return this.navigationService.navigateToRoot();
    }

    if (this.completed === null) {
      this.logger.error('No status code was provided');
      return this.navigationService.navigateToRoot();
    }
  }

  protected performSubmission(): Observable<string | null> {
    const partyId = this.partyService.partyId;
    this.password = this.formState.password.value;

    return this.resource.createBcProviderAccount(partyId, this.password).pipe(
      tap(() => (this.completed = true)),
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

  protected afterSubmitIsSuccessful(): void {
    this.navigationService.navigateToRoot();
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
