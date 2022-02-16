import { Component, Input } from '@angular/core';

import { Subscription } from 'rxjs';

@Component({
  selector: 'ui-busy-loading',
  templateUrl: './busy-loading.component.html',
  styleUrls: ['./busy-loading.component.scss'],
})
export class BusyLoadingComponent {
  @Input() public busy?: Subscription;
  @Input() public align: 'left' | 'center';
  @Input() public loadingMessage: string;

  public constructor() {
    this.align = 'left';
    this.loadingMessage = 'Loading...';
  }

  /**
   * @description
   * Get the text alignment CSS class.
   */
  public getTextAlignment(): string {
    return `text-${this.align}`;
  }
}
