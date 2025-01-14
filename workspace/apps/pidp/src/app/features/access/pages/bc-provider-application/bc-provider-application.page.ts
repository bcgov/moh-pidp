import {
  AsyncPipe,
  NgIf,
  NgOptimizedImage,
  NgSwitch,
  NgSwitchCase,
  NgSwitchDefault,
  NgTemplateOutlet,
} from '@angular/common';
import { Element } from '@angular/compiler';
import {
  Component,
  Inject,
  OnInit,
  TemplateRef,
  ViewChild,
} from '@angular/core';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogConfig } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatTooltipModule } from '@angular/material/tooltip';
import { ActivatedRoute, Router } from '@angular/router';

import {
  EMPTY,
  Observable,
  catchError,
  exhaustMap,
  map,
  of,
  switchMap,
  tap,
} from 'rxjs';

import { FaIconComponent } from '@fortawesome/angular-fontawesome';
import { faCircleRight } from '@fortawesome/free-regular-svg-icons';
import {
  faAngleRight,
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
  CrossFieldErrorMatcher,
  DialogOptions,
  HtmlComponent,
  InjectViewportCssClassDirective,
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
import { BreadcrumbComponent } from '@app/shared/components/breadcrumb/breadcrumb.component';
import { NeedHelpComponent } from '@app/shared/components/need-help/need-help.component';
import { DialogBcproviderCreateComponent } from '@app/shared/components/success-dialog/components/dialog-bcprovider-create.component';
import { SuccessDialogComponent } from '@app/shared/components/success-dialog/success-dialog.component';

import { AccessRoutes } from '../../access.routes';
import { BcProviderApplicationFormState } from './bc-provider-application-form-state';
import { BcProviderApplicationResource } from './bc-provider-application-resource.service';

@Component({
  selector: 'app-bc-provider-application',
  templateUrl: './bc-provider-application.page.html',
  styleUrls: ['./bc-provider-application.page.scss'],
  standalone: true,
  imports: [
    AsyncPipe,
    BreadcrumbComponent,
    FaIconComponent,
    InjectViewportCssClassDirective,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    MatTooltipModule,
    NeedHelpComponent,
    NgIf,
    NgOptimizedImage,
    NgSwitch,
    NgSwitchCase,
    NgSwitchDefault,
    NgTemplateOutlet,
    ReactiveFormsModule,
    SuccessDialogComponent,
    FaIconComponent,
  ],
})
export class BcProviderApplicationPage
  extends AbstractFormPage<BcProviderApplicationFormState>
  implements OnInit
{
  public faCircleCheck = faCircleCheck;
  public faAngleRight = faAngleRight;
  public faCircleRight = faCircleRight;
  public faLockOpen = faLockOpen;
  public faUser = faUser;
  public faXmark = faXmark;
  public formState: BcProviderApplicationFormState;
  public AccessRoutes = AccessRoutes;
  public showErrorCard = false;
  public showMessageCard = false;
  public completed: boolean | null;
  public username = '';
  public password = '';
  public previousUrl = '';
  public showOverlayOnSubmit = false;
  public errorMatcher = new CrossFieldErrorMatcher();
  public componentType = DialogBcproviderCreateComponent;
  public breadcrumbsData: Array<{ title: string; path: string }> = [
    { title: 'Home', path: '' },
    {
      title: 'Access',
      path: AccessRoutes.routePath(AccessRoutes.ACCESS_REQUESTS),
    },
    { title: 'BCProvider account information', path: '' },
  ];

  public fullName$!: Observable<string>;

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
    private router: Router,
    private navigationService: NavigationService,
    private partyService: PartyService,
    private resource: BcProviderApplicationResource,
    private route: ActivatedRoute,
    private utilsService: UtilsService,
  ) {
    super(dependenciesService);
    this.formState = new BcProviderApplicationFormState(fb);
    this.previousUrl = this.navigationService.getPreviousUrl();

    const routeData = this.route.snapshot.data;
    this.completed =
      routeData.bcProviderApplicationStatusCode == StatusCode.COMPLETED;

    this.activeLayout = '';
  }

  public onBack(): void {
    this.navigationService.navigateToRoot();
  }

  public onSuccessDialogClose(): void {
    this.dialog.closeAll();
  }

  public onUplift(): void {
    const data: DialogOptions = {
      title: 'Redirecting',
      component: HtmlComponent,
      data: {
        content: this.documentService.getRedirectingToSignInNotice(),
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

    this.fullName$ = this.resource
      .getProfileStatus(this.partyService.partyId)
      .pipe(
        map(
          (profileStatus) =>
            profileStatus?.status.dashboardInfo.displayFullName ?? '',
        ),
      );
  }
  public navigateTo(path: string): void {
    this.router.navigateByUrl(path);
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
        this.showErrorCard = true;
        return '';
      }),
    );
  }

  protected afterSubmitIsSuccessful(): void {
    if (
      this.previousUrl
        .split('/')
        .includes(AccessRoutes.PROVINCIAL_ATTACHMENT_SYSTEM)
    ) {
      this.router.navigate([
        AccessRoutes.BASE_PATH,
        AccessRoutes.PROVINCIAL_ATTACHMENT_SYSTEM,
      ]);
    } else {
      this.navigationService.navigateToRoot();
    }
  }

  private showSuccessDialog(): void {
    const config: MatDialogConfig = {
      disableClose: true,
    };
    this.dialog.open(this.successDialogTemplate, config);
  }

  private uplift(): Observable<void | null> {
    this.loadingOverlayService.open(LOADING_OVERLAY_DEFAULT_MESSAGE);
    return this.resource.createLinkTicket(this.partyService.partyId).pipe(
      switchMap(() => {
        this.loadingOverlayService.close();
        return this.authService.logout(
          `${
            this.config.applicationUrl +
            AuthRoutes.routePath(AuthRoutes.AUTO_LOGIN)
          }?idp_hint=${IdentityProvider.BC_PROVIDER}`,
        );
      }),
      catchError(() => {
        // TODO: what to do on error?
        this.logger.error('Link Request creation failed');

        return of(null);
      }),
    );
  }
}
