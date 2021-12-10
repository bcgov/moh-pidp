import { ChangeDetectionStrategy, Component, Input } from '@angular/core';

import { User } from '@bcgov/shared/data-access';

@Component({
  selector: 'ui-user-info',
  templateUrl: './user-info.component.html',
  styleUrls: ['./user-info.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class UserInfoComponent {
  @Input() public user: User | null | undefined;
}
