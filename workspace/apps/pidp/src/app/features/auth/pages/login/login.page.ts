import { Component, Inject } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute } from '@angular/router';

import { EMPTY, Observable, exhaustMap } from 'rxjs';

import {
  DialogOptions,
  HtmlComponent,
  PidpViewport,
  ViewportService,
} from '@bcgov/shared/ui';
import { ConfirmDialogComponent } from '@bcgov/shared/ui';

import { APP_CONFIG, AppConfig } from '@app/app.config';
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
export class LoginPage {
  public viewportOptions = PidpViewport;

  public bcscMobileSetupUrl: string;
  public isAdminLogin: boolean;
  public showOtherLoginOptions: boolean;
  public IdentityProvider = IdentityProvider;
  public providerIdentitySupport: string;

  public viewport = PidpViewport.xsmall;
  public isMobileTitleVisible = this.viewport === PidpViewport.xsmall;

  public get otherLoginOptionsIcon(): string {
    return this.showOtherLoginOptions ? 'indeterminate_check_box' : 'add_box';
  }

  public constructor(
    @Inject(APP_CONFIG) private config: AppConfig,
    private authService: AuthService,
    private route: ActivatedRoute,
    private dialog: MatDialog,
    private documentService: DocumentService,
    private viewportService: ViewportService
  ) {
    const routeSnapshot = this.route.snapshot;

    const routeData = routeSnapshot.data.loginPageData as LoginPageRouteData;

    this.bcscMobileSetupUrl = this.config.urls.bcscMobileSetup;
    this.isAdminLogin = routeData.isAdminLogin;

    this.viewportService.viewportBroadcast$.subscribe((viewport) =>
      this.onViewportChange(viewport)
    );
    this.showOtherLoginOptions = false;
    this.providerIdentitySupport = this.config.emails.providerIdentitySupport;
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
      this.login(idpHint);
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

  private login(idpHint: IdentityProvider): Observable<void> {
    const endorsementToken =
      this.route.snapshot.queryParamMap.get('endorsement-token');
    return this.authService.login({
      idpHint: idpHint,
      redirectUri:
        this.config.applicationUrl +
        (this.route.snapshot.routeConfig?.path === 'admin'
          ? '/' + AdminRoutes.MODULE_PATH
          : '') +
        (endorsementToken ? `?endorsement-token=${endorsementToken}` : ''),
    });
  }
}
