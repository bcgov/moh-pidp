import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { Observable, catchError, map, noop, of, tap } from 'rxjs';

import { PartyService } from '@app/core/party/party.service';
import { LoggerService } from '@app/core/services/logger.service';
import { UtilsService } from '@app/core/services/utils.service';
import { specialAuthorityEformsSupportEmail } from '@app/features/access/pages/sa-eforms/sa-eforms.constants';
import { AccessTokenService } from '@app/features/auth/services/access-token.service';
import { StatusCode } from '@app/features/portal/enums/status-code.enum';

import { UserAccessAgreementResource } from './user-access-agreement-resource.service';

@Component({
  selector: 'app-user-access-agreement',
  templateUrl: './user-access-agreement.page.html',
  styleUrls: ['./user-access-agreement.page.scss'],
})
export class UserAccessAgreementPage implements OnInit {
  public title: string;
  public username: Observable<string>;
  public completed: boolean | null;
  public accessRequestFailed: boolean;
  public specialAuthoritySupportEmail: string;
  public redirectUrl: string | null = null;

  public constructor(
    private route: ActivatedRoute,
    private router: Router,
    private partyService: PartyService,
    private resource: UserAccessAgreementResource,
    private logger: LoggerService,
    private utilsService: UtilsService,
    accessTokenService: AccessTokenService
  ) {
    this.title = this.route.snapshot.data.title;
    this.username = accessTokenService
      .decodeToken()
      .pipe(map((token) => token?.name ?? ''));

    const routeData = this.route.snapshot.data;
    this.completed = routeData.userAccessAgreementCode === StatusCode.COMPLETED;
    this.accessRequestFailed = false;
    this.specialAuthoritySupportEmail = specialAuthorityEformsSupportEmail;
  }

  public ngOnInit(): void {
    const partyId = this.partyService.partyId;

    if (!partyId) {
      this.logger.error('No party ID was provided');
      return this.navigateToRoot();
    }

    if (this.completed === null) {
      this.logger.error('No status code was provided');
      return this.navigateToRoot();
    }

    if (this.route.snapshot.queryParamMap.has('redirect-url')) {
      this.redirectUrl = this.route.snapshot.queryParamMap.get('redirect-url');
    }
    this.utilsService.scrollTop();
  }

  public onAcceptAgreement(): void {
    this.resource
      .acceptAgreement(this.partyService.partyId)
      .pipe(
        tap(() => {
          this.completed = true;
          if (!this.redirectUrl) {
            this.navigateToRoot();
          } else {
            this.router.navigate([this.redirectUrl]);
          }
          this.utilsService.scrollTopWithDelay();
        }),
        catchError((error: HttpErrorResponse) => {
          if (error.status === HttpStatusCode.BadRequest) {
            this.completed = false;
            return of(noop());
          }
          this.accessRequestFailed = true;
          return of(noop());
        })
      )
      .subscribe();
  }

  public onBack(): void {
    this.navigateToRoot();
  }

  private navigateToRoot(): void {
    this.router.navigate([this.route.snapshot.data.routes.root]);
  }
}
