import { STEPPER_GLOBAL_OPTIONS } from '@angular/cdk/stepper';
import { AsyncPipe, NgIf, NgOptimizedImage } from '@angular/common';
import { Component, HostListener, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';

import { Observable, map } from 'rxjs';

import { FaIconComponent } from '@fortawesome/angular-fontawesome';
import { faBell } from '@fortawesome/free-regular-svg-icons';
import { faArrowUp, faBookmark } from '@fortawesome/free-solid-svg-icons';
import { NavigationService } from '@pidp/presentation';

import {
  ConfirmDialogComponent,
  DialogOptions,
  HtmlComponent,
  InjectViewportCssClassDirective,
} from '@bcgov/shared/ui';

import { PartyService } from '@app/core/party/party.service';
import { Constants } from '@app/shared/constants';

import { AccessRoutes } from '../access/access.routes';
import { OrganizationInfoRoutes } from '../organization-info/organization-info.routes';
import { ProfileRoutes } from '../profile/profile.routes';
import { AlertCode } from './enums/alert-code.enum';
import { PortalResource } from './portal-resource.service';

@Component({
  selector: 'app-portal',
  templateUrl: './portal.page.html',
  styleUrls: ['./portal.page.scss'],
  providers: [
    {
      provide: STEPPER_GLOBAL_OPTIONS,
      useValue: { displayDefaultIndicatorType: false },
    },
  ],
  standalone: true,
  imports: [
    FaIconComponent,
    InjectViewportCssClassDirective,
    NgIf,
    NgOptimizedImage,
    AsyncPipe,
  ],
})
export class PortalPage implements OnInit {
  public faBookmark = faBookmark;
  public faArrowUp = faArrowUp;
  public showBackToTopButton: boolean = false;
  public ProfileRoutes = ProfileRoutes;
  public AccessRoutes = AccessRoutes;
  public OrganizationInfoRoutes = OrganizationInfoRoutes;
  public AlertCode = AlertCode;
  public faBell = faBell;
  public alerts$!: Observable<AlertCode[]>;
  public previousUrl = '';

  public constructor(
    private partyService: PartyService,
    private resource: PortalResource,
    private router: Router,
    private navigationService: NavigationService,
    private dialog: MatDialog,
  ) {
    this.previousUrl = this.navigationService.getPreviousUrl();
  }

  @HostListener('window:scroll', [])
  public onWindowScroll(): void {
    const scrollPosition =
      window.scrollY ||
      document.documentElement.scrollTop ||
      document.body.scrollTop ||
      0;
    const scrollThreshold = 200;
    this.showBackToTopButton = scrollPosition > scrollThreshold;
  }

  public scrollToTop(): void {
    window.scrollTo({ top: 0, behavior: 'smooth' });
  }

  public navigateTo(path: string): void {
    this.router.navigateByUrl(path);
  }

  public ngOnInit(): void {
    this.alerts$ = this.resource
      .getProfileStatus(this.partyService.partyId)
      .pipe(map((profileStatus) => profileStatus?.alerts ?? []));
    if (this.previousUrl.split('/').includes(Constants.newUserURL)) {
      this.showNewUserPopup();
    }
  }

  private showNewUserPopup(): void {
    const data: DialogOptions = {
      title: 'Account information completed',
      bottomBorder: false,
      titlePosition: 'center',
      bodyTextPosition: 'center',
      component: HtmlComponent,
      data: {
        content:
          'Thank you for providing OneHealthID with the information needed at this time, now you can explore OneHealthID to its full potential, based on your provided information.',
      },
      imageSrc: '/assets/images/online-marketing-hIgeoQjS_iE-unsplash.jpg',
      imageType: 'banner',
      width: '30rem',
      height: '26rem',
      cancelHide: true,
      actionHide: true,
      imageSizeFull: true,
      titleMarginTop: true,
      closeButton: true,
      class: 'new-dialog-container',
    };
    this.dialog
      .open(ConfirmDialogComponent, { data, disableClose: true })
      .afterClosed()
      .pipe()
      .subscribe();
  }
}
