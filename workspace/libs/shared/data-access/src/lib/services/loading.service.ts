import { Injectable } from '@angular/core';

import { BehaviorSubject, Observable } from 'rxjs';

export interface LoadingOptions {
  withMessage: boolean;
}

/**
 * @description
 * Provides a state management interface between the
 * LoadingInterceptor and the user interface.
 *
 * NOTE: No associated overlay or loading indicator
 * components exist leaving the responsibility of
 * implementing the user inteface to the application
 * or associated shared libraries.
 */
@Injectable({
  providedIn: 'root',
})
export class LoadingService {
  public readonly loading$: Observable<LoadingOptions | null>;
  private _loading$: BehaviorSubject<LoadingOptions | null>;

  public constructor() {
    this._loading$ = new BehaviorSubject<LoadingOptions | null>(null);
    this.loading$ = this._loading$.asObservable();
  }

  public show(withMessage: boolean): void {
    this.setLoading({ withMessage });
  }

  public hide(): void {
    this.setLoading(null);
  }

  private setLoading(showOrHide: LoadingOptions | null): void {
    this._loading$.next(showOrHide);
  }
}
