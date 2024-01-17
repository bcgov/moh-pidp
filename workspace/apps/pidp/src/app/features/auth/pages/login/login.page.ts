import { Component, Inject, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute } from '@angular/router';

import { EMPTY, Observable, exhaustMap, of, switchMap } from 'rxjs';

import {
  DialogOptions,
  HtmlComponent,
  PidpViewport,
  ViewportService,
} from '@bcgov/shared/ui';
import { ConfirmDialogComponent } from '@bcgov/shared/ui';

import { APP_CONFIG, AppConfig } from '@app/app.config';
import {
  ClientLogsService,
  MicrosoftLogLevel,
} from '@app/core/services/client-logs.service';
import { DocumentService } from '@app/core/services/document.service';
import { AdminRoutes } from '@app/features/admin/admin.routes';

import { IdentityProvider } from '../../enums/identity-provider.enum';
import { AuthService } from '../../services/auth.service';

export interface LoginPageRouteData {
  title: string;
  isAdminLogin: boolean;
}
@Component({
  selector: 'app-login',
  templateUrl: './login.page.html',
  styleUrls: ['./login.page.scss'],
})
export class LoginPage implements OnInit {
  public viewportOptions = PidpViewport;

  public bcscMobileSetupUrl: string;
  public isAdminLogin: boolean;
  public showOtherLoginOptions: boolean;
  public IdentityProvider = IdentityProvider;
  public providerIdentitySupport: string;

  public viewport = PidpViewport.xsmall;
  public isMobileTitleVisible = this.viewport === PidpViewport.xsmall;

  private endorsementToken: string | null = null;

  public get otherLoginOptionsIcon(): string {
    return this.showOtherLoginOptions ? 'indeterminate_check_box' : 'add_box';
  }

  public constructor(
    @Inject(APP_CONFIG) private config: AppConfig,
    private authService: AuthService,
    private clientLogsService: ClientLogsService,
    private route: ActivatedRoute,
    private dialog: MatDialog,
    private documentService: DocumentService,
    private viewportService: ViewportService,
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

    if (this.endorsementToken) {
      this.clientLogsService
        .createClientLog({
          message: `A user has landed on the login page with an endorsement request`,
          logLevel: MicrosoftLogLevel.INFORMATION,
          additionalInformation: this.endorsementToken,
        })
        .subscribe();
    }
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
    return this.createClientLogIfNeeded(idpHint).pipe(
      switchMap(() =>
        this.authService.login({
          idpHint: idpHint,
          redirectUri:
            this.config.applicationUrl +
            (this.route.snapshot.routeConfig?.path === 'admin'
              ? '/' + AdminRoutes.MODULE_PATH
              : '') +
            (this.endorsementToken
              ? `?endorsement-token=${this.endorsementToken}`
              : ''),
        }),
      ),
    );
  }
}
