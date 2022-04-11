import { Injectable } from '@angular/core';

import {
  BehaviorSubject,
  Observable,
  UnaryFunction,
  exhaustMap,
  isObservable,
  map,
  pipe,
  tap,
} from 'rxjs';

import { BusyOverlayConfig } from './models/busy-overlay-config.model';
import { BusyOverlayOptions } from './models/busy-overlay-options.model';

@Injectable({
  providedIn: 'root',
})
export class BusyService {
  private overlayConfig$: BehaviorSubject<BusyOverlayConfig>;

  public constructor() {
    this.overlayConfig$ = new BehaviorSubject<BusyOverlayConfig>(
      this.defaultConfig
    );
  }

  /**
   * @description
   * Listens for changes to the busy indicator message.
   */
  public get message$(): Observable<string> {
    return this.overlayConfig$
      .asObservable()
      .pipe(map((overlayConfig: BusyOverlayConfig) => overlayConfig.message));
  }

  /**
   * @description
   * Listens for changes to the busy indicator options.
   */
  public get options$(): Observable<BusyOverlayOptions> {
    return this.overlayConfig$
      .asObservable()
      .pipe(map((overlayConfig: BusyOverlayConfig) => overlayConfig.options));
  }

  /**
   * @description
   * Show a busy overlay message during an asychronous event through
   * the use of a custom operator that performs the asynchrous call.
   */
  public showBusyMessagePipe<T, R>(
    message: string,
    observableOrFunction: Observable<T> | ((params: R) => Observable<T>),
    options: BusyOverlayOptions = this.defaultOptions
  ): UnaryFunction<Observable<R>, Observable<T>> {
    const operator = isObservable(observableOrFunction)
      ? exhaustMap(() => observableOrFunction)
      : exhaustMap(observableOrFunction);

    return pipe(
      tap((_) => this.overlayConfig$.next({ message, options })),
      operator, // Asynchrous call being messaged
      tap((_) => this.overlayConfig$.next(this.defaultConfig))
    );
  }

  private get defaultConfig(): BusyOverlayConfig {
    return {
      message: '',
      options: this.defaultOptions,
    };
  }

  private get defaultOptions(): BusyOverlayOptions {
    return {
      overlayViewport: false,
    };
  }
}
