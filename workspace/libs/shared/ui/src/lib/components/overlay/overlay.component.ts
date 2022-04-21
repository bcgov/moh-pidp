import { ChangeDetectionStrategy, Component, Input } from '@angular/core';

@Component({
  selector: 'ui-overlay',
  templateUrl: './overlay.component.html',
  styleUrls: ['./overlay.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class OverlayComponent {
  @Input() public show: boolean | null;

  public readonly message: string;

  public constructor() {
    this.show = false;
    this.message = 'Your request is being processed';
  }
}
