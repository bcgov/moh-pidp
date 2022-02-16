import {
  ChangeDetectionStrategy,
  Component,
  EventEmitter,
  Input,
  Output,
} from '@angular/core';

@Component({
  selector: 'ui-collection-notice',
  templateUrl: './collection-notice.component.html',
  styleUrls: ['./collection-notice.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CollectionNoticeComponent {
  @Input() public show!: boolean;
  @Output() public accepted: EventEmitter<boolean>;

  public constructor() {
    this.accepted = new EventEmitter<boolean>();
  }

  public onAccept(): void {
    this.accepted.emit(true);

    this.show = false;
  }
}
