import { ChangeDetectionStrategy, Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { FaIconComponent } from '@fortawesome/angular-fontawesome';
import { faAngleRight } from '@fortawesome/free-solid-svg-icons';

import {
  InjectViewportCssClassDirective,
  PageHeaderComponent,
  PageSectionComponent,
  PageSubheaderComponent,
  TextButtonDirective,
} from '@bcgov/shared/ui';

import { userAccessAgreementTitle } from '@app/features/profile/pages/user-access-agreement/user-access-agreement-routing.routes';

@Component({
  selector: 'app-user-access-agreement-document',
  templateUrl: './user-access-agreement-document.component.html',
  styleUrls: ['./user-access-agreement-document.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  standalone: true,
  imports: [
    FaIconComponent,
    PageHeaderComponent,
    PageSectionComponent,
    InjectViewportCssClassDirective,
    PageSubheaderComponent,
    TextButtonDirective,
  ],
})
export class UserAccessAgreementDocumentComponent {
  public readonly title: string;
  public faAngleRight = faAngleRight;

  public constructor(
    private route: ActivatedRoute,
    private router: Router,
  ) {
    this.title = userAccessAgreementTitle;
  }

  public onBack(): void {
    this.navigateToRoot();
  }

  private navigateToRoot(): void {
    this.router.navigate([this.route.snapshot.data.routes.root]);
  }
}
