import { STEPPER_GLOBAL_OPTIONS } from '@angular/cdk/stepper';
import { NgIf, NgOptimizedImage } from '@angular/common';
import { Component, Inject, HostListener } from '@angular/core';

import { FaIconComponent } from '@fortawesome/angular-fontawesome';
import { faArrowUp, faArrowRight } from '@fortawesome/free-solid-svg-icons';
import {
  InjectViewportCssClassDirective
} from '@bcgov/shared/ui';

import { APP_CONFIG, AppConfig } from '@app/app.config';
import { Router } from '@angular/router';
import { ProfileRoutes } from '../profile/profile.routes';
import { AccessRoutes } from '../access/access.routes';



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
    NgOptimizedImage
  ],
})
export class PortalPage {

  public faArrowRight = faArrowRight;
  public faArrowUp = faArrowUp;
  public showBackToTopButton: boolean = false;

  public constructor(
    @Inject(APP_CONFIG) private config: AppConfig,
    private router: Router
  ) {
  }

  @HostListener('window:scroll', [])
  public onWindowScroll(): void {
    const scrollPosition = window.scrollY || document.documentElement.scrollTop || document.body.scrollTop || 0;
    const scrollThreshold = 200;
    this.showBackToTopButton = scrollPosition > scrollThreshold;
  }
  public scrollToTop(): void {
    window.scrollTo({ top: 0, behavior: 'smooth' });
  }
  public navigateToAccountLinkingPage(): void {
    this.router.navigateByUrl(ProfileRoutes.routePath(ProfileRoutes.ACCOUNT_LINKING));
  }

  public navigateToAccessPage(): void {
    this.router.navigateByUrl(AccessRoutes.routePath(AccessRoutes.ACCESS_REQUESTS));
  }
}
