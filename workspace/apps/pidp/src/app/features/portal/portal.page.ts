import { STEPPER_GLOBAL_OPTIONS } from '@angular/cdk/stepper';
import { NgIf, NgOptimizedImage } from '@angular/common';
import { Component, HostListener, Inject } from '@angular/core';
import { Router } from '@angular/router';

import { FaIconComponent } from '@fortawesome/angular-fontawesome';
import { faArrowRight, faArrowUp } from '@fortawesome/free-solid-svg-icons';

import { InjectViewportCssClassDirective } from '@bcgov/shared/ui';

import { APP_CONFIG, AppConfig } from '@app/app.config';

import { AccessRoutes } from '../access/access.routes';
import { OrganizationInfoRoutes } from '../organization-info/organization-info.routes';
import { ProfileRoutes } from '../profile/profile.routes';

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
  ],
})
export class PortalPage {
  public faArrowRight = faArrowRight;
  public faArrowUp = faArrowUp;
  public showBackToTopButton: boolean = false;
  public ProfileRoutes = ProfileRoutes;
  public AccessRoutes = AccessRoutes;
  public OrganizationInfoRoutes = OrganizationInfoRoutes;

  public constructor(
    @Inject(APP_CONFIG) private config: AppConfig,
    private router: Router,
  ) {}

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
}
