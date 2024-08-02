import { CommonModule, NgOptimizedImage } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';

import { Observable, exhaustMap, switchMap, tap } from 'rxjs';

import {
  LOADING_OVERLAY_DEFAULT_MESSAGE,
  LoadingOverlayService,
} from '@pidp/presentation';

import {
  ConfirmDialogComponent,
  DialogOptions,
  HtmlComponent,
  InjectViewportCssClassDirective,
} from '@bcgov/shared/ui';

import { User } from '@app/features/auth/models/user.model';
import { ProfileRoutes } from '@app/features/profile/profile.routes';
import { BreadcrumbComponent } from '@app/shared/components/breadcrumb/breadcrumb.component';
import { SuccessDialogComponent } from '@app/shared/components/success-dialog/success-dialog.component';

import { IdentityProvider } from '../../enums/identity-provider.enum';
import { BcProviderUser } from '../../models/bc-provider-user.model';
import { AuthorizedUserService } from '../../services/authorized-user.service';
import { LinkAccountConfirmResource } from './link-account-confirm-resource.service';
import { AccessRoutes } from '@app/features/access/access.routes';

@Component({
  selector: 'app-link-account-confirm',
  standalone: true,
  imports: [
    BreadcrumbComponent,
    CommonModule,
    InjectViewportCssClassDirective,
    MatButtonModule,
    NgOptimizedImage,
    SuccessDialogComponent,
  ],
  templateUrl: './link-account-confirm.page.html',
  styleUrl: './link-account-confirm.page.scss',
})
export class LinkAccountConfirmPage implements OnInit {
  public user$: Observable<User>;
  public breadcrumbsData: Array<{ title: string; path: string }> = [
    {title: 'Home', path: ''},
    {
      title: 'Access',
      path: AccessRoutes.routePath(AccessRoutes.ACCESS_REQUESTS),
    },
    {title: 'Link Account', path: ''},
  ];

  public showInstructions: boolean = false;
  public constructor(
    private dialog: MatDialog,
    private authorizedUserService: AuthorizedUserService,
    private linkAccountConfirmResource: LinkAccountConfirmResource,
    private router: Router,
    private loadingOverlayService: LoadingOverlayService,
  ) {
    this.user$ = this.authorizedUserService.user$;
  }

  public ngOnInit(): void {
    this.user$
      .pipe(
        switchMap((user) => {
          const data: DialogOptions = {
            title: 'Account linking',
            titlePosition: 'center',
            bottomBorder: false,
            bodyTextPosition: 'center',
            component: HtmlComponent,
            data: {
              content: `Your existing OneHealthID profile is about to be linked to ${this.getPendingAccountDescription(
                user,
              )}. Is this information correct?`,
            },
            imageSrc:
              '/assets/images/online-marketing-hIgeoQjS_iE-unsplash.jpg',
            imageType: 'banner',
            width: '31.25rem',
            height: '26rem',
            actionText: 'Correct',
            actionTypePosition: 'center',
            class: 'dialog-container dialog-padding',
          };
          return this.dialog
            .open(ConfirmDialogComponent, {
              data,
              disableClose: true,
            })
            .afterClosed()
            .pipe(
              exhaustMap((result) =>
                result
                  ? this.link()
                  : this.linkAccountConfirmResource.cancelLink(),
              ),
            );
        }),
      )
      .subscribe();
  }

  private link(): Observable<number> {
    this.loadingOverlayService.open(LOADING_OVERLAY_DEFAULT_MESSAGE);
    return this.linkAccountConfirmResource.linkAccount().pipe(
      tap(() => {
        this.loadingOverlayService.close();
        this.router.navigate([
          ProfileRoutes.routePath(ProfileRoutes.ACCOUNT_LINKING),
        ]);
      }),
    );
  }

  public toggleInstructions(): void {
    this.showInstructions = !this.showInstructions;
  }

  private getPendingAccountDescription(user: User): string {
    switch (user.identityProvider) {
      case IdentityProvider.BCSC:
        return `the BC Services Card account ${user.firstName} ${user.lastName}`;
      case IdentityProvider.PHSA:
        return `the PHSA account ${user.email}`;
      case IdentityProvider.BC_PROVIDER: {
        const idpId = (user as BcProviderUser).idpId;
        const accountName = idpId.endsWith('@bcp') ? idpId.slice(0, -4) : idpId;
        return `the BC Provider account ${accountName}`;
      }
      default:
        return 'a new account';
    }
  }
}
