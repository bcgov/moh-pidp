import { CommonModule, NgOptimizedImage } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';

import { Observable, exhaustMap, switchMap, tap } from 'rxjs';

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

import { User } from '@app/features/auth/models/user.model';
import { ProfileRoutes } from '@app/features/profile/profile.routes';
import { SuccessDialogComponent } from '@app/shared/components/success-dialog/success-dialog.component';

import { AuthorizedUserService } from '../../services/authorized-user.service';
import { LinkAccountConfirmResource } from './link-account-confirm-resource.service';

@Component({
  selector: 'app-link-account-confirm',
  standalone: true,
  imports: [
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
  public constructor(
    private dialog: MatDialog,
    private authorizedUserService: AuthorizedUserService,
    private navigationService: NavigationService,
    private linkAccountConfirmResource: LinkAccountConfirmResource,
    private router: Router,
    private loadingOverlayService: LoadingOverlayService,
  ) {
    this.user$ = this.authorizedUserService.user$;
  }

  public onBack(): void {
    this.navigateToRoot();
  }

  public ngOnInit(): void {
    this.user$
      .pipe(
        switchMap((user) => {
          const accountName = user.email ? `, email: ${user.email}` : '';
          const data: DialogOptions = {
            title: 'Confirmation Required',
            component: HtmlComponent,
            data: {
              content: `Are you sure you want to link to
              ${user.identityProvider === 'bcsc' ? 'BCSC' : ''}
              ${user.firstName} ${user.lastName}${accountName}?`,
            },
          };
          return this.dialog
            .open(ConfirmDialogComponent, { data, disableClose: true })
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

  private navigateToRoot(): void {
    this.navigationService.navigateToRoot();
  }
}
