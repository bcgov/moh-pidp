import { Component, Inject } from '@angular/core';

import { InstanceConfigHolderService } from 'ng-busy';

@Component({
  selector: 'ui-busy-overlay-message',
  templateUrl: './busy-overlay-message.component.html',
  styleUrls: ['./busy-overlay-message.component.scss'],
})
export class BusyOverlayMessageComponent {
  public constructor(
    @Inject('instanceConfigHolder')
    private instanceConfigHolder: InstanceConfigHolderService
  ) {}

  public get message(): string | undefined {
    return this.instanceConfigHolder.config.message;
  }
}
