import { AfterViewInit, Component, OnDestroy, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import {
  ActivatedRoute,
  Data,
  Event,
  NavigationEnd,
  Router,
  RouterOutlet,
  Scroll,
} from '@angular/router';

import {
  Observable,
  Subject,
  delay,
  filter,
  map,
  mergeMap,
  takeUntil,
} from 'rxjs';

import { contentContainerSelector } from '@bcgov/shared/ui';

import { RouteStateService } from '@core/services/route-state.service';
import { SnowplowService } from '@core/services/snowplow.service';
import { UtilsService } from '@core/services/utils.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
  standalone: true,
  imports: [RouterOutlet],
})
export class AppComponent implements OnInit, AfterViewInit, OnDestroy {
  private readonly destroy$ = new Subject<void>();

  public constructor(
    private readonly activatedRoute: ActivatedRoute,
    private readonly titleService: Title,
    private readonly routeStateService: RouteStateService,
    private readonly utilsService: UtilsService,
    private readonly router: Router,
    private readonly snowplowService: SnowplowService,
  ) {
    this.router.events.subscribe((evt) => {
      if (evt instanceof NavigationEnd) {
        this.snowplowService.trackPageView();
      }
    });
  }

  public ngOnInit(): void {
    this.setPageTitle(this.routeStateService.onNavigationEnd());
    this.handleRouterScrollEvents(this.routeStateService.onScrollEvent());
  }

  public ngAfterViewInit(): void {
    this.router.events
      .pipe(
        filter((event: Event) => event instanceof NavigationEnd),
        delay(0),
        takeUntil(this.destroy$),
      )
      .subscribe(() => {
        this.snowplowService.trackPageView();
      });
  }

  public ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
  /**
   * @description
   * Set the HTML page <title> on route event.
   */
  private setPageTitle(routeEvent: Observable<Event>): void {
    routeEvent
      .pipe(
        // Swap what is being observed to the activated route
        map(() => this.activatedRoute),
        // Find the last activated route by traversing over the state tree, and
        // then return it to the stream
        map((route: ActivatedRoute) => {
          while (route.firstChild) {
            route = route.firstChild;
          }
          return route;
        }),
        mergeMap((route: ActivatedRoute) => route.data),
        takeUntil(this.destroy$),
      )
      .subscribe((routeData: Data) =>
        this.titleService.setTitle(routeData.title),
      );
  }

  /**
   * @description
   * Handle the scrolling of the content container
   * based on a triggered scroll event.
   */
  private handleRouterScrollEvents(scroll: Observable<Scroll>): void {
    scroll
      .pipe(
        map((event: Scroll) => event.anchor ?? null),
        delay(500), // Provide settling time before triggering scroll
      )
      .subscribe((routeFragment: string | null) =>
        routeFragment
          ? this.utilsService.scrollToAnchor(routeFragment)
          : this.utilsService.scrollTop(contentContainerSelector),
      );
  }
}
