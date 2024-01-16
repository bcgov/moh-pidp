import { ChangeDetectionStrategy, Component, Input } from '@angular/core';

import { User } from '@bcgov/shared/data-access';

import { KeyValueInfoOrientation, KeyValueInfoComponent } from '../key-value-info/key-value-info.component';
import { FullnamePipe } from '../../pipes/fullname.pipe';
import { DefaultPipe } from '../../pipes/default.pipe';

@Component({
    selector: 'ui-user-info',
    templateUrl: './user-info.component.html',
    styleUrls: ['./user-info.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
    standalone: true,
    imports: [
        KeyValueInfoComponent,
        DefaultPipe,
        FullnamePipe,
    ],
})
export class UserInfoComponent {
  @Input() public user: User | null | undefined;
  public mode: KeyValueInfoOrientation;

  public constructor() {
    this.mode = 'horizontal';
  }
}
