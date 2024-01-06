import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { MatDialogConfig } from '@angular/material/dialog';

import { catchError, noop, of, tap } from 'rxjs';

import { faCircleCheck, faXmark } from '@fortawesome/free-solid-svg-icons';
import { NavigationService } from '@pidp/presentation';

import { NoContent } from '@bcgov/shared/data-access';
import { CrossFieldErrorMatcher } from '@bcgov/shared/ui';

import {
  AbstractFormDependenciesService,
  AbstractFormPage,
} from '@app/core/classes/abstract-form-page.class';
import { PartyService } from '@app/core/party/party.service';

import { BcProviderEditFormState } from './bc-provider-edit-form-state';
import {
  BcProviderChangePasswordRequest,
  BcProviderEditResource,
} from './bc-provider-edit-resource.service';

export interface BcProviderEditInitialStateModel {
  bcProviderId: string;
}

@Component({
  selector: 'app-bc-provider-edit',
  templateUrl: './bc-provider-edit.page.html',
  styleUrls: ['./bc-provider-edit.page.scss'],
})
export class BcProviderEditPage
  extends AbstractFormPage<BcProviderEditFormState>
  implements OnInit
{
  public faCircleCheck = faCircleCheck;
  public faXmark = faXmark;
  public formState: BcProviderEditFormState;
  public showErrorCard = false;
  public errorCardText = '';
  public showMessageCard = false;
  public messageCardText = '';
  public username = '';
  public errorMatcher = new CrossFieldErrorMatcher();

  // ui-page is handling this.
  public showOverlayOnSubmit = false;

  @ViewChild('successDialog')
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  public successDialogTemplate!: TemplateRef<any>;

  public get isResetButtonEnabled(): boolean {
    return this.formState.form.valid;
  }

  public constructor(
    dependenciesService: AbstractFormDependenciesService,
    fb: FormBuilder,
    private navigationService: NavigationService,
    private partyService: PartyService,
    private resource: BcProviderEditResource,
  ) {
    super(dependenciesService);
    this.formState = new BcProviderEditFormState(fb);
  }

  public onBack(): void {
    this.navigationService.navigateToRoot();
  }

  public onSuccessDialogClose(): void {
    this.dialog.closeAll();
    this.navigationService.navigateToRoot();
  }

  public ngOnInit(): void {
    const partyId = this.partyService.partyId;

    this.resource
      .get(partyId)
      .subscribe((bcProviderObject: BcProviderEditInitialStateModel) => {
        this.username = bcProviderObject.bcProviderId;
      });
  }

  protected performSubmission(): NoContent {
    const data: BcProviderChangePasswordRequest = {
      partyId: this.partyService.partyId,
      newPassword: this.formState.newPassword.value,
    };

    return this.resource.changePassword(data).pipe(
      tap((_) => {
        this.setError('');
        this.showSuccessDialog();
      }),
      catchError(() => {
        const message = 'An error occurred.';
        this.setError(message);
        this.setMessage('');
        return of(noop());
      }),
    );
  }

  // Do I need this
  private setError(message: string): void {
    this.showErrorCard = !!message;
    this.errorCardText = message;
  }

  // Do I need this
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
