import {
  NgFor,
  NgIf,
  NgSwitch,
  NgSwitchCase,
  NgSwitchDefault,
} from '@angular/common';
import { ChangeDetectionStrategy, Component, Input } from '@angular/core';

import { LookupCodePipe } from '@app/modules/lookup/lookup-code.pipe';

@Component({
  selector: 'app-profile-card-summary-content',
  templateUrl: './profile-card-summary-content.component.html',
  styleUrls: ['./profile-card-summary-content.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  standalone: true,
  imports: [
    NgIf,
    NgFor,
    NgSwitch,
    NgSwitchCase,
    NgSwitchDefault,
    LookupCodePipe,
  ],
})
export class ProfileCardSummaryContentComponent {
  @Input() public properties?:
    | { key: string; value: string | number; label?: string }[]
    | null;
}
