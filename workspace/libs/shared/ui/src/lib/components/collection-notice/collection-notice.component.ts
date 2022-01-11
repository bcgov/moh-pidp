import {
  Component,
  ChangeDetectionStrategy,
  Input,
  Output,
  EventEmitter,
} from '@angular/core';

@Component({
  selector: 'ui-collection-notice',
  templateUrl: './collection-notice.component.html',
  styleUrls: ['./collection-notice.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CollectionNoticeComponent {
  @Input() public show!: boolean;
  @Output() public close: EventEmitter<boolean>;

  public constructor() {
    this.close = new EventEmitter<boolean>();
  }

  public onClose(checked: boolean): void {
    if (checked) {
      this.close.emit(checked);
    }

    this.show = false;
  }
}
