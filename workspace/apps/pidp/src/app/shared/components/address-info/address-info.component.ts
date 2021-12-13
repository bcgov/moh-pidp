import { ChangeDetectionStrategy, Component, Input } from '@angular/core';

import { Address } from '@bcgov/shared/data-access';

@Component({
  selector: 'app-address-info',
  templateUrl: './address-info.component.html',
  styleUrls: ['./address-info.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AddressInfoComponent {
  @Input() public address: Address | null | undefined;
}
