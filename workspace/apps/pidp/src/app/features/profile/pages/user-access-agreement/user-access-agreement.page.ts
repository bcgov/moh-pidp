import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { EMPTY, Observable, map } from 'rxjs';

import { PartyService } from '@app/core/party/party.service';
import { AccessTokenService } from '@app/features/auth/services/access-token.service';

import { UserAccessAgreementResourceService } from './user-access-agreement-resource.service';

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
    private partyService: PartyService,
    private resource: UserAccessAgreementResourceService,
    accessTokenService: AccessTokenService
  ) {
    this.title = this.route.snapshot.data.title;
    this.username = accessTokenService
      .decodeToken()
      .pipe(map((token) => token?.name ?? ''));
  }

  public onSubmit(): void {
    const partyId = this.partyService.partyId;

    partyId ? this.resource.acceptAgreement(partyId).subscribe() : EMPTY;
  }

  public onBack(): void {
    this.navigateToRoot();
  }

  private navigateToRoot(): void {
    this.router.navigate([this.route.snapshot.data.routes.root]);
  }
}
