import { ChangeDetectionStrategy, Component, Input } from '@angular/core';

import { User } from '@bcgov/shared/data-access';

import { KeyValueInfoOrientation } from '../key-value-info/key-value-info.component';

@Component({
  selector: 'ui-user-info',
  templateUrl: './user-info.component.html',
  styleUrls: ['./user-info.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class UserInfoComponent {
  @Input() public user: User | null | undefined;
  public mode: KeyValueInfoOrientation;

  public constructor() {
    this.mode = 'horizontal';
  }
}
