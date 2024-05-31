import { STEPPER_GLOBAL_OPTIONS } from '@angular/cdk/stepper';
import { NgIf, NgOptimizedImage } from '@angular/common';
import { Component, Inject, HostListener } from '@angular/core';

import { FaIconComponent } from '@fortawesome/angular-fontawesome';
import { faArrowUp, faArrowRight } from '@fortawesome/free-solid-svg-icons';
import {
  InjectViewportCssClassDirective
} from '@bcgov/shared/ui';

import { APP_CONFIG, AppConfig } from '@app/app.config';



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
}
