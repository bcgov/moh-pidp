import { Component } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { ActivatedRoute, Router } from '@angular/router';

import {
  PageComponent,
  PageFooterComponent,
  PageHeaderComponent,
  PageSectionComponent,
  PageSectionSubheaderComponent,
  YesNoContentComponent,
} from '@bcgov/shared/ui';

@Component({
  selector: 'app-site-privacy-security-checklist',
  templateUrl: './site-privacy-security-checklist.page.html',
  styleUrls: ['./site-privacy-security-checklist.page.scss'],
  standalone: true,
  imports: [
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    PageComponent,
    PageFooterComponent,
    PageHeaderComponent,
    PageSectionComponent,
    PageSectionSubheaderComponent,
    YesNoContentComponent,
  ],
})
export class SitePrivacySecurityChecklistPage {
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
