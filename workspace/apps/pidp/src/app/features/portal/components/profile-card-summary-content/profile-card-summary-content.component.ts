import {
  NgFor,
  NgIf,
  NgSwitch,
  NgSwitchCase,
  NgSwitchDefault,
} from '@angular/common';
import { ChangeDetectionStrategy, Component, Input } from '@angular/core';

import { PhonePipe } from 'libs/shared/ui/src/lib/pipes/phone.pipe';

import { LookupCodePipe } from '../../../../modules/lookup/lookup-code.pipe';

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
    PhonePipe,
  ],
})
export class ProfileCardSummaryContentComponent {
  @Input() public properties?:
    | { key: string; value: string | number; label?: string }[]
    | null;
}
