import { Component } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { ActivatedRoute, Router } from '@angular/router';

import {
  AlertComponent,
  PageComponent,
  PageFooterActionDirective,
  PageFooterComponent,
  PageHeaderComponent,
  PageSectionComponent,
  PageSectionSubheaderComponent,
} from '@bcgov/shared/ui';

@Component({
  selector: 'app-compliance-training',
  templateUrl: './compliance-training.page.html',
  styleUrls: ['./compliance-training.page.scss'],
  standalone: true,
  imports: [
    AlertComponent,
    MatButtonModule,
    PageComponent,
    PageFooterActionDirective,
    PageFooterComponent,
    PageHeaderComponent,
    PageSectionComponent,
    PageSectionSubheaderComponent,
  ],
})
export class ComplianceTrainingPage {
  public title: string;

  public constructor(
    private route: ActivatedRoute,
    private router: Router,
  ) {
    this.title = this.route.snapshot.data.title;
  }

  public onSubmit(): void {
    this.navigateToRoot();
  }

  public onBack(): void {
    this.navigateToRoot();
  }

  private navigateToRoot(): void {
    this.router.navigate([this.route.snapshot.data.routes.root]);
  }
}
