import { ChangeDetectionStrategy, Component, Input } from '@angular/core';

import { Address } from '@bcgov/shared/data-access';
import {
  DefaultPipe,
  KeyValueInfoComponent,
  PostalPipe,
} from '@bcgov/shared/ui';

import { LookupCodePipe } from '@app/modules/lookup/lookup-code.pipe';

@Component({
  selector: 'app-address-info',
  templateUrl: './address-info.component.html',
  styleUrls: ['./address-info.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  standalone: true,
  imports: [DefaultPipe, KeyValueInfoComponent, LookupCodePipe, PostalPipe],
})
export class AddressInfoComponent {
  @Input() public address: Address | null | undefined;
}
