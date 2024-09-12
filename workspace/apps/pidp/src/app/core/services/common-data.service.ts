import { Injectable } from '@angular/core';

import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class CommonDataService {
  private pushEventSource = new BehaviorSubject('');

  public pushEvent = this.pushEventSource.asObservable();
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  public publishEvent(event: any): void {
    this.pushEventSource.next(event);
  }
}
