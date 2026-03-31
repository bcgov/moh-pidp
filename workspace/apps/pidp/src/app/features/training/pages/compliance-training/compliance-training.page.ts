import { Component } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { ActivatedRoute, Router } from '@angular/router';

import {
  AlertComponent,
  AlertContentDirective,
  PageComponent,
  PageFooterActionDirective,
  PageFooterComponent,
  PageHeaderComponent,
  PageSectionComponent,
  PageSectionSubheaderComponent,
  PageSectionSubheaderDescDirective,
} from '@bcgov/shared/ui';

@Component({
  selector: 'app-compliance-training',
  templateUrl: './compliance-training.page.html',
  styleUrls: ['./compliance-training.page.scss'],
  standalone: true,
  imports: [
    AlertComponent,
    AlertContentDirective,
    MatButtonModule,
    PageComponent,
    PageFooterActionDirective,
    PageFooterComponent,
    PageHeaderComponent,
    PageSectionComponent,
    PageSectionSubheaderComponent,
    PageSectionSubheaderDescDirective,
  ],
})
export class ComplianceTrainingPage {
  public title: string;

  public constructor(
    private readonly route: ActivatedRoute,
    private readonly router: Router,
  ) {
    this.title = this.route.snapshot.data.title;
  }

  public onSubmit(): void {
    this.navigateToRoot();
  }

  private navigateToRoot(): void {
    this.router.navigate([this.route.snapshot.data.routes.root]);
  }
}
