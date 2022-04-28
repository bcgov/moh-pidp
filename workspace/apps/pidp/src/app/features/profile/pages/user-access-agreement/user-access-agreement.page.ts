import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-user-access-agreement',
  templateUrl: './user-access-agreement.page.html',
  styleUrls: ['./user-access-agreement.page.scss'],
})
export class UserAccessAgreementPage {
  public title: string;

  public constructor(private route: ActivatedRoute, private router: Router) {
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
