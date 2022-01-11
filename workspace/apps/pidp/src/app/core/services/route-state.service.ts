import { Injectable } from '@angular/core';
import {
  Router,
  RouterEvent,
  Event,
  NavigationStart,
  NavigationEnd,
  NavigationCancel,
  NavigationError,
} from '@angular/router';

import { Observable } from 'rxjs';

import { filter } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class RouteStateService {
  public constructor(private router: Router) {}

  /**
   * @description
   * Listener for the route navigation start event.
   */
  public onNavigationStart(): Observable<Event> {
    return this.router.events.pipe(
      filter(
        (event: Event): event is RouterEvent => event instanceof NavigationStart
      )
    );
  }

  /**
   * @description
   * Listener for the route navigation stop events.
   */
  public onNavigationStop(): Observable<Event> {
    return this.router.events.pipe(
      filter(
        (event: Event): event is RouterEvent =>
          event instanceof NavigationEnd ||
          event instanceof NavigationCancel ||
          event instanceof NavigationError
      )
    );
  }

  /**
   * @description
   * Listener for the route navigation end event.
   */
  public onNavigationEnd(): Observable<Event> {
    return this.router.events.pipe(
      filter(
        (event: Event): event is RouterEvent => event instanceof NavigationEnd
      )
    );
  }
}
