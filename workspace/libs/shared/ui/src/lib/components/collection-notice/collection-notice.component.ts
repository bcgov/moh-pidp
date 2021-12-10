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
  @Output() public remove: EventEmitter<boolean>;

  public constructor() {
    this.remove = new EventEmitter<boolean>();
  }

  public onClose(checked: boolean): void {
    if (checked) {
      this.remove.emit(checked);
    }

    this.show = false;
  }
}
