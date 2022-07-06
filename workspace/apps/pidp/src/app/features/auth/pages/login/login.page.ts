import { Component, Inject } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';

import { EMPTY, Observable, exhaustMap } from 'rxjs';

import {
  DashboardHeaderConfig,
  DialogOptions,
  HtmlComponent,
} from '@bcgov/shared/ui';
import { ConfirmDialogComponent } from '@bcgov/shared/ui';

import { APP_CONFIG, AppConfig } from '@app/app.config';
import { DocumentService } from '@app/core/services/document.service';

import { IdentityProvider } from '../../enums/identity-provider.enum';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.page.html',
  styleUrls: ['./login.page.scss'],
})
export class LoginPage {
  public title: string;
  public headerConfig: DashboardHeaderConfig;
  public loginCancelled: boolean;
  public bcscSupportUrl: string;
  public bcscMobileSetupUrl: string;
  public specialAuthorityUrl: string;
  public providerIdentitySupportEmail: string;
  public idpHint: IdentityProvider;

  public IdentityProvider = IdentityProvider;

  public constructor(
    @Inject(APP_CONFIG) private config: AppConfig,
    private authService: AuthService,
    private route: ActivatedRoute,
    private router: Router,
    private dialog: MatDialog,
    private documentService: DocumentService
  ) {
    const routeSnapshot = this.route.snapshot;

    this.title = routeSnapshot.data.title;
    this.headerConfig = { theme: 'dark', allowMobileToggle: false };
    this.loginCancelled = routeSnapshot.queryParams.action === 'cancelled';
    this.bcscSupportUrl = this.config.urls.bcscSupport;
    this.bcscMobileSetupUrl = this.config.urls.bcscMobileSetup;
    this.specialAuthorityUrl = this.config.urls.specialAuthority;
    this.providerIdentitySupportEmail =
      this.config.emails.providerIdentitySupport;
    this.idpHint = routeSnapshot.data.idpHint;
  }

  public onScrollToAnchor(): void {
    this.router.navigate([], {
      fragment: 'systems',
      queryParamsHandling: 'preserve',
    });
  }

  public onLogin(idpHint?: IdentityProvider): void {
    if (this.idpHint === IdentityProvider.IDIR) {
      this.login(this.idpHint);
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
      .pipe(
        exhaustMap((result) =>
          result ? this.login(idpHint ?? this.idpHint) : EMPTY
        )
      )
      .subscribe();
  }

  private login(idpHint: IdentityProvider): Observable<void> {
    const endorsementToken =
      this.route.snapshot.queryParamMap.get('endorsement-token');
    return this.authService.login({
      idpHint: idpHint,
      redirectUri:
        this.config.applicationUrl +
        (endorsementToken ? `?endorsement-token=${endorsementToken}` : ''),
    });
  }
}
