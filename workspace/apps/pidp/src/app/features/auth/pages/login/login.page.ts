import { Component, Inject } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';

import { EMPTY, Observable, exhaustMap } from 'rxjs';

import {
  DashboardHeaderConfig,
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

  public headerConfig: DashboardHeaderConfig;
  public loginCancelled: boolean;
  public bcscSupportUrl: string;
  public bcscMobileSetupUrl: string;
  public specialAuthorityUrl: string;
  public providerIdentitySupportEmail: string;
  public isAdminLogin: boolean;
  public prescriptionRenewalSupportUrl: string;
  public bcscAppDownload: string;

  public IdentityProvider = IdentityProvider;

  public viewport = PidpViewport.xsmall;
  public isMobileTitleVisible = this.viewport === PidpViewport.xsmall;
  public isWebTitleVisible = this.viewport !== PidpViewport.xsmall;
  public isPidpLogoVisible = this.viewport !== PidpViewport.xsmall;
  public hcimWebHeaderColor: 'white' | 'grey' = 'grey';

  public constructor(
    @Inject(APP_CONFIG) private config: AppConfig,
    private authService: AuthService,
    private route: ActivatedRoute,
    private router: Router,
    private dialog: MatDialog,
    private documentService: DocumentService,
    private viewportService: ViewportService
  ) {
    const routeSnapshot = this.route.snapshot;

    const routeData = routeSnapshot.data.loginPageData as LoginPageRouteData;

    this.headerConfig = { theme: 'dark', allowMobileToggle: false };
    this.loginCancelled = routeSnapshot.queryParams.action === 'cancelled';
    this.bcscSupportUrl = this.config.urls.bcscSupport;
    this.bcscMobileSetupUrl = this.config.urls.bcscMobileSetup;
    this.specialAuthorityUrl = this.config.urls.specialAuthority;
    this.providerIdentitySupportEmail =
      this.config.emails.providerIdentitySupport;
    this.isAdminLogin = routeData.isAdminLogin;
    this.prescriptionRenewalSupportUrl = this.config.urls.prescriptionRenewal;
    this.bcscAppDownload = this.config.urls.bcscAppDownload;

    this.viewportService.viewportBroadcast$.subscribe((viewport) =>
      this.onViewportChange(viewport)
    );
  }
  private onViewportChange(viewport: PidpViewport): void {
    this.viewport = viewport;

    switch (this.viewport) {
      case PidpViewport.xsmall:
        this.isMobileTitleVisible = true;
        this.isWebTitleVisible = false;
        this.isPidpLogoVisible = false;
        this.hcimWebHeaderColor = 'grey';
        break;
      case PidpViewport.small:
      case PidpViewport.medium:
      case PidpViewport.large:
        this.isMobileTitleVisible = false;
        this.isWebTitleVisible = true;
        this.isPidpLogoVisible = true;
        this.hcimWebHeaderColor = 'white';
        break;
      default:
        throw 'not implemented: ' + this.viewport;
    }
  }

  public onScrollToAnchor(): void {
    this.router.navigate([], {
      fragment: 'systems',
      queryParamsHandling: 'preserve',
    });
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
