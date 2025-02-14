import {
  AsyncPipe,
  CommonModule,
  NgFor,
  NgIf,
  NgOptimizedImage,
} from '@angular/common';
import { Component, Inject, OnDestroy, OnInit } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatDialog } from '@angular/material/dialog';
import { MatTooltipModule } from '@angular/material/tooltip';
import { ActivatedRoute, Router } from '@angular/router';

import {
  EMPTY,
  Observable,
  Subject,
  catchError,
  exhaustMap,
  of,
  switchMap,
  takeUntil,
} from 'rxjs';

import {
  LOADING_OVERLAY_DEFAULT_MESSAGE,
  LoadingOverlayService,
  NavigationService,
} from '@pidp/presentation';

import {
  ConfirmDialogComponent,
  DialogOptions,
  HtmlComponent,
  InjectViewportCssClassDirective,
} from '@bcgov/shared/ui';

import { APP_CONFIG, AppConfig } from '@app/app.config';
import { PartyService } from '@app/core/party/party.service';
import { DocumentService } from '@app/core/services/document.service';
import { LoggerService } from '@app/core/services/logger.service';
import { UtilsService } from '@app/core/services/utils.service';
import { AuthRoutes } from '@app/features/auth/auth.routes';
import { IdentityProvider } from '@app/features/auth/enums/identity-provider.enum';
import { AuthService } from '@app/features/auth/services/auth.service';
import { AuthorizedUserService } from '@app/features/auth/services/authorized-user.service';
import { StatusCode } from '@app/features/portal/enums/status-code.enum';
import { SuccessDialogComponent } from '@app/shared/components/success-dialog/success-dialog.component';

import { AccountLinkingResource } from './account-linking-resource.service';
import { linkedAccountCardText } from './account-linking.constants';
import { Credential } from './account-linking.model';
import { BreadcrumbComponent } from '@app/shared/components/breadcrumb/breadcrumb.component';
import { AccessRoutes } from '@app/features/access/access.routes';

@Component({
  selector: 'app-account-linking',
  standalone: true,
  imports: [
    BreadcrumbComponent,
    CommonModule,
    InjectViewportCssClassDirective,
    MatButtonModule,
    NgOptimizedImage,
    SuccessDialogComponent,
    AsyncPipe,
    MatTooltipModule,
    NgFor,
    NgIf,
  ],
  templateUrl: './account-linking.page.html',
  styleUrl: './account-linking.page.scss',
})
export class AccountLinkingPage implements OnInit, OnDestroy {
  public title: string;
  public completed: boolean | null;
  public redirectUrl: string | null = null;
  public identityProvider$: Observable<IdentityProvider>;
  public IdentityProvider = IdentityProvider;
  public credentials$: Observable<Credential[]>;
  public credentials: Credential[] = [];
  public linkedAccountsIdp: IdentityProvider[] = [];
  public showInstructions: boolean = false;
  private unsubscribe$ = new Subject<void>();

  public constructor(
    @Inject(APP_CONFIG) private config: AppConfig,
    private route: ActivatedRoute,
    private router: Router,
    private partyService: PartyService,
    private logger: LoggerService,
    private utilsService: UtilsService,
    private authorizedUserService: AuthorizedUserService,
    private documentService: DocumentService,
    private resource: AccountLinkingResource,
    private authService: AuthService,
    private dialog: MatDialog,
    private loadingOverlayService: LoadingOverlayService,
    private navigationService: NavigationService,
  ) {
    this.title = this.route.snapshot.data.title;
    const partyId = this.partyService.partyId;

    const routeData = this.route.snapshot.data;
    this.completed = routeData.accountLinking === StatusCode.COMPLETED;
    this.identityProvider$ = this.authorizedUserService.identityProvider$;
    this.credentials$ = this.resource.getCredentials(partyId);
  }

  public toggleInstructions(): void {
    this.showInstructions = !this.showInstructions;
  }
  public breadcrumbsData: Array<{ title: string; path: string }> = [
    {title: 'Home', path: ''},
    {
      title: 'Access',
      path: AccessRoutes.routePath(AccessRoutes.ACCESS_REQUESTS),
    },
    {title: 'Account Linking', path: ''},
  ];

  public onLinkAccount(idpHint: IdentityProvider): void {
    const data: DialogOptions = {
      title: 'You will be redirected',
      bottomBorder: false,
      titlePosition: 'center',
      bodyTextPosition: 'center',
      component: HtmlComponent,
      data: {
        content:
          'You will need to sign in with the credentials of the account you want to link.',
      },
      imageSrc: '/assets/images/online-marketing-hIgeoQjS_iE-unsplash.jpg',
      imageType: 'banner',
      width: '31rem',
      height: '24rem',
      actionText: 'Continue',
      actionTypePosition: 'center',
      class: 'dialog-container dialog-padding',
    };
    this.dialog
      .open(ConfirmDialogComponent, { data })
      .afterClosed()
      .pipe(
        exhaustMap((result) => (result ? this.linkRequest(idpHint) : EMPTY)),
      )
      .subscribe();
  }

  public getCardText(idp: IdentityProvider): string {
    return linkedAccountCardText[idp] ?? '';
  }

  public hasCredential(idp: IdentityProvider): boolean {
    return this.credentials.some((c) => c.identityProvider === idp);
  }

  public ngOnInit(): void {
    const partyId = this.partyService.partyId;

    if (!partyId) {
      this.logger.error('No party ID was provided');
      return this.navigateToRoot();
    }

    if (this.completed === null) {
      this.logger.error('No status code was provided');
      return this.navigateToRoot();
    }

    if (this.route.snapshot.queryParamMap.has('redirect-url')) {
      this.redirectUrl = this.route.snapshot.queryParamMap.get('redirect-url');
    }
    this.utilsService.scrollTop();

    this.handleLinkedAccounts();
  }

  public ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }

  private handleLinkedAccounts(): void {
    this.credentials$
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe((credentials) => {
        this.credentials = credentials;
      });
  }

  private linkRequest(idpHint: IdentityProvider): Observable<void | null> {
    this.loadingOverlayService.open(LOADING_OVERLAY_DEFAULT_MESSAGE);
    return this.resource
      .createLinkTicket(this.partyService.partyId, idpHint)
      .pipe(
        switchMap(() => {
          this.loadingOverlayService.close();
          return this.authService.logout(
            `${
              this.config.applicationUrl +
              AuthRoutes.routePath(AuthRoutes.AUTO_LOGIN)
            }?idp_hint=${idpHint}`,
          );
        }),
        catchError(() => {
          // TODO: what to do on error?
          this.logger.error('Link Request creation failed');

          return of(null);
        }),
      );
  }

  navigateTo() {
    this.hasCredential(IdentityProvider.BC_PROVIDER) ? this.onPageNavigate(['/access/bc-provider-edit']) :
    this.onPageNavigate(['/access/bc-provider-application'])
  }

  public onPageNavigate(url: string[]): void {
    this.router.navigate(url);
  }
  private navigateToRoot(): void {
    this.navigationService.navigateToRoot();
  }
}
