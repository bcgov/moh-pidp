import { Component, EventEmitter, Input, Output } from '@angular/core';

import { Subscription } from 'rxjs';

@Component({
  selector: 'ui-busy-overlay',
  templateUrl: './busy-overlay.component.html',
  styleUrls: ['./busy-overlay.component.scss'],
})
export class BusyOverlayComponent {
  /**
   * @description
   * Busy subscription for use when blocking content from
   * being interacted with in the template. For example,
   * during but not limited to HTTP requests.
   */
  @Input() public busy?: Subscription;
  @Output() public started: EventEmitter<boolean>;
  @Output() public stopped: EventEmitter<boolean>;

  public constructor() {
    this.started = new EventEmitter<boolean>();
    this.stopped = new EventEmitter<boolean>();
  }

  /**
   * @description
   * Indicate the busy overlay is displayed.
   */
  public onBusyStart(event: boolean): void {
    this.started.emit(event);
  }

  /**
   * @description
   * Indicate the busy overlay is hidden.
   */
  public onBusyStop(event: boolean): void {
    this.stopped.emit(event);
  }
}
