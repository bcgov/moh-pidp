import { NgIf } from '@angular/common';
import {
  ChangeDetectionStrategy,
  Component,
  EventEmitter,
  Input,
  Output,
} from '@angular/core';
import { MatButtonModule } from '@angular/material/button';

import { AlertActionsDirective } from '../alert/alert-actions.directive';
import { AlertContentDirective } from '../alert/alert-content.directive';
import { AlertComponent } from '../alert/alert.component';

@Component({
  selector: 'ui-collection-notice',
  templateUrl: './collection-notice.component.html',
  styleUrls: ['./collection-notice.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  standalone: true,
  imports: [
    AlertActionsDirective,
    AlertComponent,
    AlertContentDirective,
    MatButtonModule,
    NgIf,
  ],
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
