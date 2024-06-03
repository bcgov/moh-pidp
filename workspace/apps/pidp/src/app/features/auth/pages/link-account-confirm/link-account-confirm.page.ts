import { CommonModule, NgOptimizedImage } from '@angular/common';
import { Component, Inject, OnInit } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatDialog } from '@angular/material/dialog';

import { Observable, exhaustMap, switchMap, tap } from 'rxjs';

import { NavigationService } from '@pidp/presentation';

import {
  ConfirmDialogComponent,
  DialogOptions,
  HtmlComponent,
  InjectViewportCssClassDirective,
} from '@bcgov/shared/ui';

import { APP_CONFIG, AppConfig } from '@app/app.config';
import { User } from '@app/features/auth/models/user.model';
import { SuccessDialogComponent } from '@app/shared/components/success-dialog/success-dialog.component';

import { AuthService } from '../../services/auth.service';
import { AuthorizedUserService } from '../../services/authorized-user.service';
import { LinkAccountConfirmResourceService } from './link-account-confirm-resource.service';

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
    @Inject(APP_CONFIG) private config: AppConfig,
    private dialog: MatDialog,
    private authService: AuthService,
    private authorizedUserService: AuthorizedUserService,
    private navigationService: NavigationService,
    private linkAccountConfirmResource: LinkAccountConfirmResourceService,
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
          console.log(user);
          const data: DialogOptions = {
            title: 'Confirmation Required',
            component: HtmlComponent,
            data: {
              content: `Are you sure you want to link to
              ${user.firstName} ${user.lastName} ${user.email ?? ''}?`,
            },
          };
          return this.dialog
            .open(ConfirmDialogComponent, { data, disableClose: true })
            .afterClosed()
            .pipe(
              exhaustMap((result) =>
                result
                  ? this.link()
                  : this.authService.logout(`${this.config.applicationUrl}`),
              ),
            );
        }),
      )
      .subscribe();
  }

  private link(): Observable<number> {
    return this.linkAccountConfirmResource
      .linkAccount()
      .pipe(tap(() => this.navigateToRoot()));
  }

  private navigateToRoot(): void {
    this.navigationService.navigateToRoot();
  }
}
