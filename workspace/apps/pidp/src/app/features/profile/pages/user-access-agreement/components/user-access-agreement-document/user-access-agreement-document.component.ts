import { ChangeDetectionStrategy, Component } from '@angular/core';

import { InjectViewportCssClassDirective } from '@bcgov/shared/ui';

import { userAccessAgreementTitle } from '@app/features/profile/pages/user-access-agreement/user-access-agreement-routing.routes';
import { BreadcrumbComponent } from '@app/shared/components/breadcrumb/breadcrumb.component';

@Component({
  selector: 'app-user-access-agreement-document',
  templateUrl: './user-access-agreement-document.component.html',
  styleUrls: ['./user-access-agreement-document.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [BreadcrumbComponent, InjectViewportCssClassDirective],
})
export class UserAccessAgreementDocumentComponent {
  public readonly title: string;
  public breadcrumbsData: Array<{ title: string; path: string }> = [
    { title: 'Home', path: '' },
    { title: 'UAA', path: '' },
  ];

  public constructor() {
    this.title = userAccessAgreementTitle;
  }
}
