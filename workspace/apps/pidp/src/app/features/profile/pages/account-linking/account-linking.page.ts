import { AsyncPipe, CommonModule, NgOptimizedImage } from '@angular/common';
import { Component, Inject, OnInit } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';

import {
  EMPTY,
  Observable,
  catchError,
  exhaustMap,
  map,
  of,
  switchMap,
} from 'rxjs';

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

@Component({
  selector: 'app-account-linking',
  standalone: true,
  imports: [
    CommonModule,
    InjectViewportCssClassDirective,
    MatButtonModule,
    NgOptimizedImage,
    SuccessDialogComponent,
    AsyncPipe,
  ],
  templateUrl: './account-linking.page.html',
  styleUrl: './account-linking.page.scss',
})
export class AccountLinkingPage implements OnInit {
  public title: string;
  public completed: boolean | null;
  public redirectUrl: string | null = null;
  public identityProvider$: Observable<IdentityProvider>;
  public IdentityProvider = IdentityProvider;

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
  ) {
    this.title = this.route.snapshot.data.title;

    const routeData = this.route.snapshot.data;
    this.completed = routeData.accountLinking === StatusCode.COMPLETED;
    this.identityProvider$ = this.authorizedUserService.identityProvider$;
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
    const accounts = this.resource.getLinkedAccounts(partyId).pipe(
      map((response) => {
        console.log('response', response);
        return response;
      }),
    );
    console.log('I have this many linked accounts, ', accounts);
  }

  public onLinkAccount(idpHint: IdentityProvider): void {
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
      .pipe(
        exhaustMap((result) => (result ? this.linkRequest(idpHint) : EMPTY)),
      )
      .subscribe();
  }

  public onBack(): void {
    this.navigateToRoot();
  }

  private linkRequest(idpHint: IdentityProvider): Observable<void | null> {
    return this.resource
      .createLinkTicket(this.partyService.partyId, idpHint)
      .pipe(
        switchMap(() => {
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

  private navigateToRoot(): void {
    this.router.navigate([this.route.snapshot.data.routes.root]);
  }
}
