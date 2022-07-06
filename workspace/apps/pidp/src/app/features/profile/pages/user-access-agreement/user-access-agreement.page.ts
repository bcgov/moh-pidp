import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { Observable, map } from 'rxjs';

import { AccessTokenService } from '@app/features/auth/services/access-token.service';

@Component({
  selector: 'app-user-access-agreement',
  templateUrl: './user-access-agreement.page.html',
  styleUrls: ['./user-access-agreement.page.scss'],
})
export class UserAccessAgreementPage {
  public title: string;
  public username: Observable<string>;

  public constructor(
    private route: ActivatedRoute,
    private router: Router,
    accessTokenService: AccessTokenService
  ) {
    this.title = this.route.snapshot.data.title;
    this.username = accessTokenService
      .decodeToken()
      .pipe(map((token) => token?.name ?? ''));
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
