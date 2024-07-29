import { AsyncPipe, NgIf, NgOptimizedImage } from '@angular/common';
import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogConfig } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { Router } from '@angular/router';

import { EMPTY, Observable, catchError, exhaustMap, noop, of, tap } from 'rxjs';

import { faCircleCheck, faXmark } from '@fortawesome/free-solid-svg-icons';
import {
  LOADING_OVERLAY_DEFAULT_MESSAGE,
  LoadingOverlayService,
  NavigationService,
} from '@pidp/presentation';

import { NoContent } from '@bcgov/shared/data-access';
import {
  ConfirmDialogComponent,
  CrossFieldErrorMatcher,
  DialogOptions,
  HtmlComponent,
  InjectViewportCssClassDirective,
  TextButtonDirective,
} from '@bcgov/shared/ui';

import {
  AbstractFormDependenciesService,
  AbstractFormPage,
} from '@app/core/classes/abstract-form-page.class';
import { PartyService } from '@app/core/party/party.service';
import { IdentityProvider } from '@app/features/auth/enums/identity-provider.enum';
import { AuthorizedUserService } from '@app/features/auth/services/authorized-user.service';
import { FaqRoutes } from '@app/features/faq/faq.routes';
import { NeedHelpComponent } from '@app/shared/components/need-help/need-help.component';
import { DialogBcproviderEditComponent } from '@app/shared/components/success-dialog/components/dialog-bcprovider-edit.component';
import { SuccessDialogComponent } from '@app/shared/components/success-dialog/success-dialog.component';

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
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    NeedHelpComponent,
    NgIf,
    ReactiveFormsModule,
    SuccessDialogComponent,
    NgOptimizedImage,
    TextButtonDirective,
    AsyncPipe,
  ],
})
export class BcProviderEditPage
  extends AbstractFormPage<BcProviderEditFormState>
  implements OnInit
{
  public faCircleCheck = faCircleCheck;
  public faXmark = faXmark;
  public formState: BcProviderEditFormState;
  public showErrorCard = false;
  public username = '';
  public errorMatcher = new CrossFieldErrorMatcher();
  public componentType = DialogBcproviderEditComponent;
  public identityProvider$: Observable<IdentityProvider>;
  public IdentityProvider = IdentityProvider;

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
    private router: Router,
    private loadingOverlayService: LoadingOverlayService,
    private authorizedUserService: AuthorizedUserService,
  ) {
    super(dependenciesService);
    this.formState = new BcProviderEditFormState(fb);
    this.identityProvider$ = this.authorizedUserService.identityProvider$;
  }

  public onBack(): void {
    this.navigationService.navigateToRoot();
  }

  public onSuccessDialogClose(): void {
    this.dialog.closeAll();
    this.navigationService.navigateToRoot();
  }

  public onResetMfa(): void {
    const data: DialogOptions = {
      title: 'Multi-factor authentication (MFA) reset',
      bottomBorder: false,
      titlePosition: 'center',
      bodyTextPosition: 'center',
      component: HtmlComponent,
      data: {
        content:
          'At your request we are about to reset your MFA, this action cannot be undone. Are you sure you want to do this?',
      },
      imageSrc: '/assets/images/online-marketing-hIgeoQjS_iE-unsplash.jpg',
      imageType: 'banner',
      width: '31rem',
      height: '24rem',
      actionText: 'Continue',
      actionTypePosition: 'center',
    };
    this.dialog
      .open(ConfirmDialogComponent, { data })
      .afterClosed()
      .pipe(
        exhaustMap((result) =>
          result ? this.resetMfa(this.partyService.partyId) : EMPTY,
        ),
      )
      .subscribe();
  }

  public onLearnMore(): void {
    this.router.navigateByUrl(FaqRoutes.BASE_PATH);
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
        this.showSuccessDialog();
      }),
      catchError(() => {
        this.showErrorCard = true;
        return of(noop());
      }),
    );
  }

  private resetMfa(partyId: number): Observable<void | null> {
    const data: DialogOptions = {
      title: 'Your Multi-factor authentication (MFA) has been reset',
      bottomBorder: false,
      titlePosition: 'center',
      bodyTextPosition: 'center',
      component: HtmlComponent,
      data: {
        content:
          'Your authentication credentials have been successfully reset.',
      },
      imageSrc: '/assets/images/online-marketing-hIgeoQjS_iE-unsplash.jpg',
      imageType: 'banner',
      width: '31rem',
      height: '24rem',
      actionText: 'Continue',
      actionTypePosition: 'center',
    };

    this.loadingOverlayService.open(LOADING_OVERLAY_DEFAULT_MESSAGE);
    this.resource
      .resetMfa(partyId)
      .pipe(
        tap((_) => {
          this.loadingOverlayService.close();
          this.dialog.open(ConfirmDialogComponent, { data });
        }),
        catchError(() => {
          this.showErrorCard = true;
          return of(noop());
        }),
      )
      .subscribe();
    return of(null);
  }

  private showSuccessDialog(): void {
    const config: MatDialogConfig = {
      disableClose: true,
    };
    this.dialog.open(this.successDialogTemplate, config);
  }
}
