import { ChangeDetectionStrategy, Component, Input } from '@angular/core';

@Component({
  selector: 'app-profile-card-summary-content',
  templateUrl: './profile-card-summary-content.component.html',
  styleUrls: ['./profile-card-summary-content.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ProfileCardSummaryContentComponent {
  @Input() public properties?:
    | { key: string; value: string | number; label?: string }[]
    | null;
}
