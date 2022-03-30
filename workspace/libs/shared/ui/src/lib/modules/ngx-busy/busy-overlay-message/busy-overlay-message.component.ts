import { Component } from '@angular/core';

import { Observable, map } from 'rxjs';

import { BusyService } from '../busy.service';
import { BusyOverlayOptions } from '../models/busy-overlay-options.model';

@Component({
  selector: 'ui-busy-overlay-message',
  templateUrl: './busy-overlay-message.component.html',
  styleUrls: ['./busy-overlay-message.component.scss'],
})
export class BusyOverlayMessageComponent {
  public constructor(private busyService: BusyService) {}

  /**
   * @description
   * Message to be shown when displaying the
   * busy indicator.
   */
  public get message$(): Observable<string> {
    return this.busyService.message$;
  }

  /**
   * @description
   * Whether the viewport should be covered by the
   * overlay when displaying the busy indicator.
   */
  public get overlayViewport$(): Observable<boolean> {
    return this.busyService.options$.pipe(
      map(
        (busyOverlayOptions: BusyOverlayOptions) =>
          busyOverlayOptions.overlayViewport
      )
    );
  }
}
