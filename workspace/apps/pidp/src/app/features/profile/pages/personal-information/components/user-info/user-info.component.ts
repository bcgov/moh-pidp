import { NgIf } from '@angular/common';
import { ChangeDetectionStrategy, Component, Input } from '@angular/core';

import * as ui from '@bcgov/shared/ui';

import { User } from '@app/features/auth/models/user.model';

@Component({
    selector: 'app-user-info',
    templateUrl: './user-info.component.html',
    styleUrls: ['./user-info.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
    imports: [
        ui.DefaultPipe,
        ui.FormatDatePipe,
        ui.KeyValueInfoComponent,
        NgIf,
        ui.UserInfoComponent,
    ]
})
export class UserInfoComponent {
  @Input() public user: User | null | undefined;
}
