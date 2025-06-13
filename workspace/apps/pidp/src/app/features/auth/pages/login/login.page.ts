import { NgIf, NgOptimizedImage, NgTemplateOutlet } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { AfterViewInit, Component, Inject, OnInit } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute } from '@angular/router';

import { EMPTY, Observable, exhaustMap, of, switchMap } from 'rxjs';

import {
  AnchorDirective,
  ConfirmDialogComponent,
  DialogOptions,
  ExpansionPanelComponent,
  HtmlComponent,
  InjectViewportCssClassDirective,
  LayoutHeaderFooterComponent,
  PidpViewport,
  ViewportService,
} from '@bcgov/shared/ui';

import { APP_CONFIG, AppConfig } from '@app/app.config';
import {
  ClientLogsService,
  MicrosoftLogLevel,
} from '@app/core/services/client-logs.service';
import { DocumentService } from '@app/core/services/document.service';
import { LoggerService } from '@app/core/services/logger.service';
import { SnowplowService } from '@app/core/services/snowplow.service';
import { AdminRoutes } from '@app/features/admin/admin.routes';
import { BannerComponent } from '@app/shared/components/banner/banner.component';
import { NeedHelpComponent } from '@app/shared/components/need-help/need-help.component';

import { IdentityProvider } from '../../enums/identity-provider.enum';
import { AuthService } from '../../services/auth.service';
import { LinkAccountConfirmResource } from '../link-account-confirm/link-account-confirm-resource.service';
import { BannerFindResponse } from './banner-find.response.model';
import { LoginResource } from './login-resource.service';
import { component } from './login.constants';

export interface LoginPageRouteData {
  title: string;
  isAdminLogin: boolean;
}
@Component({
  selector: 'app-login',
  templateUrl: './login.page.html',
  styleUrls: ['./login.page.scss'],
  standalone: true,
  imports: [
    AnchorDirective,
    ExpansionPanelComponent,
    InjectViewportCssClassDirective,
    LayoutHeaderFooterComponent,
    MatButtonModule,
    NeedHelpComponent,
    NgIf,
    NgOptimizedImage,
    NgTemplateOutlet,
    BannerComponent,
  ],
})
export class LoginPage implements OnInit, AfterViewInit {
  public viewportOptions = PidpViewport;

  public bcscMobileSetupUrl: string;
  public isAdminLogin: boolean;
  public showOtherLoginOptions: boolean;
  public IdentityProvider = IdentityProvider;
  public providerIdentitySupport: string;
  public banners: Array<{ header: string; body: string; status: string }> = [];

  public viewport = PidpViewport.xsmall;
  public isMobileTitleVisible = this.viewport === PidpViewport.xsmall;

  private endorsementToken: string | null = null;
  private emailVerificationToken: string | null = null;

  public get otherLoginOptionsIcon(): string {
    return this.showOtherLoginOptions ? 'indeterminate_check_box' : 'add_box';
  }

  public constructor(
    @Inject(APP_CONFIG) private readonly config: AppConfig,
    private readonly authService: AuthService,
    private readonly clientLogsService: ClientLogsService,
    private readonly route: ActivatedRoute,
    private readonly dialog: MatDialog,
    private readonly documentService: DocumentService,
    private readonly viewportService: ViewportService,
    private readonly linkAccountConfirmResource: LinkAccountConfirmResource,
    private readonly snowplowService: SnowplowService,
    private readonly loginResource: LoginResource,
    private readonly logger: LoggerService,
  ) {
    const routeSnapshot = this.route.snapshot;

    const routeData = routeSnapshot.data.loginPageData as LoginPageRouteData;

    this.bcscMobileSetupUrl = this.config.urls.bcscMobileSetup;
    this.isAdminLogin = routeData.isAdminLogin;

    this.viewportService.viewportBroadcast$.subscribe((viewport) =>
      this.onViewportChange(viewport),
    );
    this.showOtherLoginOptions = false;
    this.providerIdentitySupport = this.config.emails.providerIdentitySupport;
  }

  public ngOnInit(): void {
    this.endorsementToken =
      this.route.snapshot.queryParamMap.get('endorsement-token');

    this.emailVerificationToken = this.route.snapshot.queryParamMap.get(
      'email-verification-token',
    );

    if (this.endorsementToken) {
      this.clientLogsService
        .createClientLog({
          message: `A user has landed on the login page with an endorsement request`,
          logLevel: MicrosoftLogLevel.INFORMATION,
          additionalInformation: this.endorsementToken,
        })
        .subscribe();
    }

    this.loginResource.findBanners(component).subscribe(
      (data: BannerFindResponse[]) => {
        this.banners = data;
      },
      (err: HttpErrorResponse) => {
        this.logger.error(
          '[LoginResource::findBanners] error has occurred: ',
          err,
        );
      },
    );
  }

  public ngAfterViewInit(): void {
    // refresh link urls now that we set the links
    this.snowplowService.refreshLinkClickTracking();
  }

  private onViewportChange(viewport: PidpViewport): void {
    this.viewport = viewport;

    switch (this.viewport) {
      case PidpViewport.xsmall:
        this.isMobileTitleVisible = true;
        break;
      case PidpViewport.small:
      case PidpViewport.medium:
      case PidpViewport.large:
        this.isMobileTitleVisible = false;
        break;
      default:
        throw 'not implemented: ' + this.viewport;
    }
  }

  public onShowOtherLoginOptions(): void {
    this.showOtherLoginOptions = !this.showOtherLoginOptions;
  }

  public onLogin(idpHint: IdentityProvider): void {
    if (idpHint === IdentityProvider.IDIR) {
      this.login(idpHint).subscribe();
      return;
    }

    const data: DialogOptions = {
      title: 'Collection Notice',
      component: HtmlComponent,
      data: {
        content: this.documentService.getPIdPCollectionNotice(),
      },
      width: '500px',
    };
    this.dialog
      .open(ConfirmDialogComponent, { data })
      .afterClosed()
      .pipe(exhaustMap((result) => (result ? this.login(idpHint) : EMPTY)))
      .subscribe();
  }

  private createClientLogIfNeeded(idpHint: IdentityProvider): Observable<void> {
    return this.endorsementToken
      ? this.clientLogsService.createClientLog({
          message: `A user has clicked on the login button with an endorsement request and the identity provider "${idpHint}"`,
          logLevel: MicrosoftLogLevel.INFORMATION,
          additionalInformation: this.endorsementToken,
        })
      : of(undefined);
  }

  private login(idpHint: IdentityProvider): Observable<void> {
    return this.linkAccountConfirmResource.cancelLink().pipe(
      switchMap(() => this.createClientLogIfNeeded(idpHint)),
      switchMap(() =>
        this.authService.login({
          idpHint: idpHint,
          redirectUri:
            this.config.applicationUrl +
            (this.route.snapshot.routeConfig?.path === 'admin'
              ? '/' + AdminRoutes.BASE_PATH
              : '') +
            (this.endorsementToken
              ? `?endorsement-token=${this.endorsementToken}`
              : '') +
            (this.emailVerificationToken
              ? `?email-verification-token=${this.emailVerificationToken}`
              : ''),
        }),
      ),
    );
  }
}
