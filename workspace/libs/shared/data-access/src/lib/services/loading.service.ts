import { Injectable } from '@angular/core';

import { BehaviorSubject, Observable } from 'rxjs';

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
  public readonly loading$: Observable<boolean>;
  private _loading$: BehaviorSubject<boolean>;

  public constructor() {
    this._loading$ = new BehaviorSubject<boolean>(false);
    this.loading$ = this._loading$.asObservable();
  }

  public show(): void {
    this.setLoading(true);
  }

  public hide(): void {
    this.setLoading(false);
  }

  private setLoading(showOrHide: boolean): void {
    this._loading$.next(showOrHide);
  }
}
