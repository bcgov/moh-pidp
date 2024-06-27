import { BreakpointObserver, BreakpointState } from '@angular/cdk/layout';
import { Injectable } from '@angular/core';

import { BehaviorSubject, Observable, map } from 'rxjs';

/**
 * @description
 * Breakpoints based on those used by the Bootstrap Grid.
 */
export const BootstrapBreakpoints = {
  xsmall: '(min-width: 0px)', // Mobile
  small: '(min-width: 576px)',
  medium: '(min-width: 768px)', // Tablet
  large: '(min-width: 992px)', // Desktop
  xlarge: '(min-width: 1200px)',
  xxlarge: '(min-width: 1400px)',
  mobile: '(min-width: 0px) and (max-width: 767.98px)',
  tablet: '(min-width: 768px) and (max-width: 991.98px)',
};
export const PidpBreakpoints = {
  xsmall: '(min-width: 0px) and (max-width: 767px)',
  small: '(min-width: 768px) and (max-width: 1023px)',
  medium: '(min-width: 1024px) and (max-width: 1199px)',
  large: '(min-width: 1200px)',
};
export enum PidpViewport {
  /**
   * Roughly corresponds to mobile
   */
  xsmall,
  /**
   * Roughly corresponds to tablet in portrait mode
   */
  small,
  /**
   * Roughly corresponds to tablet in landscape mode
   */
  medium,
  /**
   * Roughly corresponds to desktop
   */
  large,
}
@Injectable({
  providedIn: 'root',
})
export class ViewportService {
  public breakpointObserver$: Observable<BreakpointState>;

  public pidpBreakpointObserver$: Observable<BreakpointState>;
  public viewport = PidpViewport.xsmall;
  public viewportSubject = new BehaviorSubject<PidpViewport>(this.viewport);
  public viewportBroadcast$ = this.viewportSubject.asObservable();

  public constructor(
    private breakpointObserver: BreakpointObserver,
    private pidpBreakpointObserver: BreakpointObserver,
  ) {
    this.breakpointObserver$ = breakpointObserver.observe([
      BootstrapBreakpoints.medium,
      BootstrapBreakpoints.large,
      BootstrapBreakpoints.mobile,
      BootstrapBreakpoints.tablet,
    ]);

    // Set up the breakpoint observer.
    this.pidpBreakpointObserver$ = this.pidpBreakpointObserver.observe([
      PidpBreakpoints.xsmall,
      PidpBreakpoints.small,
      PidpBreakpoints.medium,
      PidpBreakpoints.large,
    ]);

    // Subscribe to the breakpoint observer.
    this.pidpBreakpointObserver$.subscribe(() => this.onBreakpointChange());
  }

  private onBreakpointChange(): void {
    if (this.pidpBreakpointObserver.isMatched(PidpBreakpoints.xsmall)) {
      this.setViewport(PidpViewport.xsmall);
    } else if (this.pidpBreakpointObserver.isMatched(PidpBreakpoints.small)) {
      this.setViewport(PidpViewport.small);
    } else if (this.pidpBreakpointObserver.isMatched(PidpBreakpoints.medium)) {
      this.setViewport(PidpViewport.medium);
    } else {
      this.setViewport(PidpViewport.large);
    }
  }
  private setViewport(viewport: PidpViewport): void {
    // If the viewport has changed, notify observers.
    if (this.viewport !== viewport) {
      this.viewport = viewport;
      this.viewportSubject.next(this.viewport);
    }
  }

  public get isMobileBreakpoint(): boolean {
    return this.breakpointObserver.isMatched(BootstrapBreakpoints.mobile);
  }

  public get isMobileBreakpoint$(): Observable<boolean> {
    return this.breakpointObserver$.pipe(
      map(
        (result: BreakpointState) =>
          result.matches && result.breakpoints[BootstrapBreakpoints.mobile],
      ),
    );
  }

  public get isTabletBreakpoint(): boolean {
    return this.breakpointObserver.isMatched(BootstrapBreakpoints.tablet);
  }

  public get isTabletBreakpoint$(): Observable<boolean> {
    return this.breakpointObserver$.pipe(
      map(
        (result: BreakpointState) =>
          result.matches && result.breakpoints[BootstrapBreakpoints.tablet],
      ),
    );
  }

  public get isTabletAndUpBreakpoint(): boolean {
    return this.breakpointObserver.isMatched(BootstrapBreakpoints.medium);
  }

  public get isTabletAndUpBreakpoint$(): Observable<boolean> {
    return this.breakpointObserver$.pipe(
      map(
        (result: BreakpointState) =>
          result.matches && result.breakpoints[BootstrapBreakpoints.medium],
      ),
    );
  }

  public get isDesktopAndUpBreakpoint(): boolean {
    return this.breakpointObserver.isMatched(BootstrapBreakpoints.large);
  }

  public get isDesktopAndUpBreakpoint$(): Observable<boolean> {
    return this.breakpointObserver$.pipe(
      map(
        (result: BreakpointState) =>
          result.matches && result.breakpoints[BootstrapBreakpoints.large],
      ),
    );
  }
}
