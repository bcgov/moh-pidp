import { ChangeDetectionStrategy, Component, Input } from '@angular/core';

import { BcscUser } from '@app/features/auth/models/bcsc-user.model';

@Component({
  selector: 'app-bcsc-user-info',
  templateUrl: './bcsc-user-info.component.html',
  styleUrls: ['./bcsc-user-info.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class BcscUserInfoComponent {
  @Input() public bcscUser: BcscUser | null | undefined;
}
