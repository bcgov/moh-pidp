import { CommonModule, NgOptimizedImage } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';

import { EMPTY, Observable, exhaustMap, switchMap, tap } from 'rxjs';

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
import { IdentityProvider } from '../../enums/identity-provider.enum';
import { faAngleRight } from '@fortawesome/free-solid-svg-icons';
import { FaIconComponent } from '@fortawesome/angular-fontawesome';


@Component({
  selector: 'app-link-account-confirm',
  standalone: true,
  imports: [
    FaIconComponent,
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
  public IdentityProvider = IdentityProvider;
  public faAngleRight = faAngleRight;
  public showInstructions:boolean=false;
  public userIdentityProvider: string = ''
  public showSucessBC: boolean = false;
  public showSucessHealth: boolean = false;
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
          this.userIdentityProvider = user.identityProvider;
          this.showSucessBC =  this.userIdentityProvider === 'bcsc' || this.showSucessBC ? true : false;
          this.showSucessHealth =  this.userIdentityProvider !== 'bcsc' || this.showSucessHealth ? true : false;
          const data: DialogOptions = {
            title: 'Account linking',
            titlePosition: 'center',
            bottomBorder: false,
            bodyTextPosition: 'center',
            component: HtmlComponent,
            data: {
              content: `Your ${user.email} is about to be linked to
              ${user.identityProvider === 'bcsc' ? 'BCSC' : ''}
              ${user.firstName} ${user.lastName} is this information correct?`,
            },
            imageSrc: '../../../assets/images/online-marketing-hIgeoQjS_iE-unsplash.jpg',
            imageType: 'banner',
            width: '31.25rem',
            height: '26rem',
            actionText:'Correct',
            actionTypePosition: 'center'
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
        this.userIdentityProvider === 'bcsc' ? this.showSucessBC = true : this.showSucessHealth = true;
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

public toggleInstructions(): void {
  this.showInstructions= !this.showInstructions;
}


public onLinkAccount(idpHint: IdentityProvider): void {
    const data: DialogOptions = {
      // title: 'Account Linking',
      // message: 'Your BCSC Hawkeye Pierce is about to be linked to hawkeyepierce@phsa.ca is this information correct ?',
      title: 'You will be redirected',
      bottomBorder: false,
      titlePosition: 'center',
      bodyTextPosition: 'center',
      component: HtmlComponent,
      data: {
        content: 'You will need to sign in with the credentials of the account you want to link.',
      },
      imageSrc: '../../../assets/images/online-marketing-hIgeoQjS_iE-unsplash.jpg',
      imageType: 'banner',
      width: '31.25rem',
      height: '26rem',
      actionText:'Continue',
      actionTypePosition: 'center'

    };
    this.dialog
      .open(ConfirmDialogComponent, { data })
      .afterClosed()
      .pipe(
        exhaustMap((result) => (result ? '' : EMPTY)),
      )
      .subscribe();
  }
}
