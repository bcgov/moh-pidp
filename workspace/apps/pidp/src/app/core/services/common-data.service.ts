import { Injectable } from '@angular/core';

import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class CommonDataService {
  private pushEventSource = new BehaviorSubject('');

  pushEvent = this.pushEventSource.asObservable();

  publishEvent(event: any) {
    this.pushEventSource.next(event);
  }
}
