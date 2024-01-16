import { ChangeDetectionStrategy, Component } from '@angular/core';

import { SharedUiModule } from '@bcgov/shared/ui';

import { userAccessAgreementTitle } from '@app/features/profile/pages/user-access-agreement/user-access-agreement-routing.module';

@Component({
  selector: 'app-user-access-agreement-document',
  templateUrl: './user-access-agreement-document.component.html',
  styleUrls: ['./user-access-agreement-document.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  standalone: true,
  imports: [SharedUiModule],
})
export class UserAccessAgreementDocumentComponent {
  public readonly title: string;

  public constructor() {
    this.title = userAccessAgreementTitle;
  }
}
