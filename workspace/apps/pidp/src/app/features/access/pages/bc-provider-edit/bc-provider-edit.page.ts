import { NgIf, NgOptimizedImage } from '@angular/common';
import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogConfig } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { Router } from '@angular/router';

import { catchError, noop, of, tap } from 'rxjs';

import { FaIconComponent } from '@fortawesome/angular-fontawesome';
import {
  faAngleRight,
  faCircleCheck,
  faXmark,
} from '@fortawesome/free-solid-svg-icons';
import { NavigationService } from '@pidp/presentation';

import { NoContent } from '@bcgov/shared/data-access';
import {
  CrossFieldErrorMatcher,
  InjectViewportCssClassDirective,
  TextButtonDirective,
} from '@bcgov/shared/ui';

import {
  AbstractFormDependenciesService,
  AbstractFormPage,
} from '@app/core/classes/abstract-form-page.class';
import { PartyService } from '@app/core/party/party.service';
import { NeedHelpComponent } from '@app/shared/components/need-help/need-help.component';
import { DialogBcproviderEditComponent } from '@app/shared/components/success-dialog/components/dialog-bcprovider-edit.component';
import { SuccessDialogComponent } from '@app/shared/components/success-dialog/success-dialog.component';

import { AccessRoutes } from '../../access.routes';
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
  standalone: true,
  imports: [
    InjectViewportCssClassDirective,
    FaIconComponent,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    NeedHelpComponent,
    NgIf,
    ReactiveFormsModule,
    SuccessDialogComponent,
    TextButtonDirective,
    NgOptimizedImage,
  ],
})
export class BcProviderEditPage
  extends AbstractFormPage<BcProviderEditFormState>
  implements OnInit
{
  public faCircleCheck = faCircleCheck;
  public faAngleRight = faAngleRight;
  public faXmark = faXmark;
  public formState: BcProviderEditFormState;
  public AccessRoutes = AccessRoutes;
  public showErrorCard = false;
  public username = '';
  public errorMatcher = new CrossFieldErrorMatcher();
  public componentType = DialogBcproviderEditComponent;

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
    private router: Router,
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

  public navigateTo(path: string): void {
    this.router.navigateByUrl(path);
  }

  protected performSubmission(): NoContent {
    const data: BcProviderChangePasswordRequest = {
      partyId: this.partyService.partyId,
      newPassword: this.formState.newPassword.value,
    };

    return this.resource.changePassword(data).pipe(
      tap((_) => {
        this.showSuccessDialog();
      }),
      catchError(() => {
        this.showErrorCard = true;
        return of(noop());
      }),
    );
  }

  private showSuccessDialog(): void {
    const config: MatDialogConfig = {
      disableClose: true,
    };
    this.dialog.open(this.successDialogTemplate, config);
  }
}
