import { Element } from '@angular/compiler';
import {
  Component,
  Inject,
  OnInit,
  TemplateRef,
  ViewChild,
} from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { MatDialogConfig } from '@angular/material/dialog';
import { ActivatedRoute } from '@angular/router';

import {
  EMPTY,
  Observable,
  catchError,
  exhaustMap,
  of,
  switchMap,
  tap,
} from 'rxjs';

import { faCircleRight } from '@fortawesome/free-regular-svg-icons';
import {
  faCircleCheck,
  faLockOpen,
  faUser,
  faXmark,
} from '@fortawesome/free-solid-svg-icons';
import {
  LOADING_OVERLAY_DEFAULT_MESSAGE,
  LoadingOverlayService,
  NavigationService,
} from '@pidp/presentation';

import {
  ConfirmDialogComponent,
  DialogOptions,
  HtmlComponent,
} from '@bcgov/shared/ui';

import { APP_CONFIG, AppConfig } from '@app/app.config';
import {
  AbstractFormDependenciesService,
  AbstractFormPage,
} from '@app/core/classes/abstract-form-page.class';
import { PartyService } from '@app/core/party/party.service';
import { DocumentService } from '@app/core/services/document.service';
import { LoggerService } from '@app/core/services/logger.service';
import { UtilsService } from '@app/core/services/utils.service';
import { AuthRoutes } from '@app/features/auth/auth.routes';
import { IdentityProvider } from '@app/features/auth/enums/identity-provider.enum';
import { AuthService } from '@app/features/auth/services/auth.service';
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
  public faCircleCheck = faCircleCheck;
  public faCircleRight = faCircleRight;
  public faLockOpen = faLockOpen;
  public faUser = faUser;
  public faXmark = faXmark;
  public formState: BcProviderApplicationFormState;
  public showErrorCard = false;
  public errorCardText = '';
  public showMessageCard = false;
  public messageCardText = '';
  public completed: boolean | null;
  public username = '';
  public password = '';
  public showOverlayOnSubmit = false;

  public activeLayout: 'upliftAccount' | 'createAccount' | '';

  @ViewChild('successDialog')
  public successDialogTemplate!: TemplateRef<Element>;

  public get isEnrolButtonEnabled(): boolean {
    return this.formState.form.valid;
  }

  public constructor(
    @Inject(APP_CONFIG) private config: AppConfig,
    dependenciesService: AbstractFormDependenciesService,
    fb: FormBuilder,
    private authService: AuthService,
    private documentService: DocumentService,
    private loadingOverlayService: LoadingOverlayService,
    private logger: LoggerService,
    private navigationService: NavigationService,
    private partyService: PartyService,
    private resource: BcProviderApplicationResource,
    private route: ActivatedRoute,
    private utilsService: UtilsService
  ) {
    super(dependenciesService);
    this.formState = new BcProviderApplicationFormState(fb);
    const routeData = this.route.snapshot.data;
    this.completed =
      routeData.bcProviderApplicationStatusCode == StatusCode.COMPLETED;

    this.activeLayout = '';
  }

  public onBack(): void {
    this.navigationService.navigateToRoot();
  }

  public hasPasswordRuleError(): boolean {
    return this.formState.password.hasError('invalidRequirements');
  }

  public onSuccessDialogClose(): void {
    this.dialog.closeAll();
    this.navigationService.navigateToRoot();
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

  public onUplift(): void {
    const data: DialogOptions = {
      title: 'BC Provider login Notice',
      component: HtmlComponent,
      data: {
        content: this.documentService.getLogoutLoginBCProviderNotice(),
      },
    };
    this.dialog
      .open(ConfirmDialogComponent, { data })
      .afterClosed()
      .pipe(exhaustMap((result) => (result ? this.uplift() : EMPTY)))
      .subscribe();
  }

  public setLayout(activeLayout: 'upliftAccount' | 'createAccount'): void {
    if (this.activeLayout !== activeLayout) {
      this.activeLayout = activeLayout;
      this.utilsService.scrollToAnchorWithDelay(activeLayout);
    } else {
      this.activeLayout = '';
    }
  }

  protected performSubmission(): Observable<string | void> {
    const partyId = this.partyService.partyId;
    this.password = this.formState.password.value;
    this.loadingOverlayService.open(LOADING_OVERLAY_DEFAULT_MESSAGE);

    return this.resource.createBcProviderAccount(partyId, this.password).pipe(
      tap((upn: string) => {
        this.username = upn;
        this.completed = true;
        this.loadingOverlayService.close();
        this.showSuccessDialog();
      }),
      catchError(() => {
        this.loadingOverlayService.close();
        const message = 'An error occurred.';
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

  private showSuccessDialog(): void {
    const config: MatDialogConfig = {
      disableClose: true,
    };
    this.dialog.open(this.successDialogTemplate, config);
  }

  private uplift(): Observable<void | null> {
    return this.resource.createLinkTicket(this.partyService.partyId).pipe(
      switchMap(() =>
        this.authService.logout(
          `${
            this.config.applicationUrl +
            AuthRoutes.routePath(AuthRoutes.AUTO_LOGIN)
          }?idp_hint=${IdentityProvider.BC_PROVIDER}`
        )
      ),
      catchError(() => {
        // TODO: what to do on error?
        this.logger.error('Link Request creation failed');

        return of(null);
      })
    );
  }
}
