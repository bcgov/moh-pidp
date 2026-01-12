import { Injectable } from '@angular/core';
import {
  Event,
  NavigationCancel,
  NavigationEnd,
  NavigationError,
  NavigationStart,
  Router,
  RouterEvent,
  Scroll,
} from '@angular/router';

import { Observable } from 'rxjs';
import { filter } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class RouteStateService {
  public constructor(private readonly router: Router) {}

  /**
   * @description
   * Listener for the route navigation start event.
   */
  public onNavigationStart(): Observable<RouterEvent> {
    return this.router.events.pipe(
      filter(
        (event: Event): event is NavigationStart =>
          event instanceof NavigationStart,
      ),
    );
  }

  /**
   * @description
   * Listener for the route navigation stop events.
   */
  public onNavigationStop(): Observable<RouterEvent> {
    return this.router.events.pipe(
      filter(
        (
          event: Event,
        ): event is NavigationEnd | NavigationCancel | NavigationError =>
          event instanceof NavigationEnd ||
          event instanceof NavigationCancel ||
          event instanceof NavigationError,
      ),
    );
  }

  /**
   * @description
   * Listener for the route navigation end event.
   */
  public onNavigationEnd(): Observable<NavigationEnd> {
    return this.router.events.pipe(
      filter(
        (event: Event): event is NavigationEnd =>
          event instanceof NavigationEnd,
      ),
    );
  }

  /**
   * @description
   * Listener for route scroll events.
   */
  public onScrollEvent(): Observable<Scroll> {
    return this.router.events.pipe(
      filter((event: Event): event is Scroll => event instanceof Scroll),
    );
  }
}
