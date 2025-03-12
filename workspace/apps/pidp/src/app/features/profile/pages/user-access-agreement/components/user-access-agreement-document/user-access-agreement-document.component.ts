import { ChangeDetectionStrategy, Component } from '@angular/core';

import { FaIconComponent } from '@fortawesome/angular-fontawesome';

import {
  InjectViewportCssClassDirective,
  PageHeaderComponent,
  PageSectionComponent,
  PageSubheaderComponent,
} from '@bcgov/shared/ui';

import { userAccessAgreementTitle } from '@app/features/profile/pages/user-access-agreement/user-access-agreement-routing.routes';
import { BreadcrumbComponent } from '@app/shared/components/breadcrumb/breadcrumb.component';

@Component({
    selector: 'app-user-access-agreement-document',
    templateUrl: './user-access-agreement-document.component.html',
    styleUrls: ['./user-access-agreement-document.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
    imports: [
        BreadcrumbComponent,
        FaIconComponent,
        PageHeaderComponent,
        PageSectionComponent,
        InjectViewportCssClassDirective,
        PageSubheaderComponent,
    ]
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
