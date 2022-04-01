import { ChangeDetectionStrategy, Component, Input } from '@angular/core';

import { User } from '@app/features/auth/models/user.model';

@Component({
  selector: 'app-user-info',
  templateUrl: './user-info.component.html',
  styleUrls: ['./user-info.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class UserInfoComponent {
  @Input() public user: User | null | undefined;
}
