import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { NavigationEnd, Event as NavigationEvent } from '@angular/router';

import { delay, of } from 'rxjs';

export interface ScrollToTopConfig {
  scrollDelayInMs: number;
  scrollToTopAfterNavigationEnd: boolean;
}
/**
 * Provides "scroll to top" behaviour on dynamic content.
 * To use this class, use ngIf="scrollToTopService.showContent$ | async".
 * Also call configure() in ngOnInit().
 * To trick the browser into scrolling to the top of the page, long content is initially hidden,
 * then displayed after a delay using showContent$.
 * Scroll to top behaviour with this service seems to work more often when
 * each page of dynamic content has its own route.
 */
@Injectable({ providedIn: 'root' })
export class ScrollToTopService {
  public config: ScrollToTopConfig = {
    scrollDelayInMs: 250,
    scrollToTopAfterNavigationEnd: true,
  };

  /**
   * When true, the host page should display long content.
   * When false, the host page should hide long  content.
   * Initially returns true with no delay, with the assumption being that the dependent dynamic content
   * should always be visible on intial page load.
   */
  public showContent$ = of(true);

  public constructor(private router: Router) {}

  /**
   * Call from ngOnInit().
   */
  public configure(config?: ScrollToTopConfig): void {
    if (config) {
      this.config = { ...config };
    }
    if (this.config.scrollToTopAfterNavigationEnd) {
      this.router.events.subscribe((event: NavigationEvent) => {
        if (event instanceof NavigationEnd) {
          this.scrollToTop();
        }
      });
    }
  }
  /**
   * Call to induce scroll to top behaviour by hiding content that depends on displayContent$.
   */
  public scrollToTop(): void {
    this.showContent$ = of(true).pipe(delay(this.config.scrollDelayInMs));
  }
}
