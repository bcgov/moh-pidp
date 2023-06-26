import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { Observable, catchError, map, noop, of, tap } from 'rxjs';

import { PartyService } from '@app/core/party/party.service';
import { LoggerService } from '@app/core/services/logger.service';
import { ToastService } from '@app/core/services/toast.service';
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
  public enrolmentError: boolean;

  public constructor(
    private route: ActivatedRoute,
    private router: Router,
    private partyService: PartyService,
    private resource: UserAccessAgreementResource,
    private logger: LoggerService,
    private toastService: ToastService,
    accessTokenService: AccessTokenService
  ) {
    this.title = this.route.snapshot.data.title;
    this.username = accessTokenService
      .decodeToken()
      .pipe(map((token) => token?.name ?? ''));

    const routeData = this.route.snapshot.data;
    this.completed = routeData.userAccessAgreementCode === StatusCode.COMPLETED;
    this.accessRequestFailed = false;
    this.enrolmentError = false;
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
  }

  public onAcceptAgreement(): void {
    this.resource
      .acceptAgreement(this.partyService.partyId)
      .pipe(
        tap(() => {
          this.completed = true;
          this.enrolmentError = false;
          this.toastService.openSuccessToast(
            'User access agreement has been accepted'
          );
        }),
        catchError((error: HttpErrorResponse) => {
          if (error.status === HttpStatusCode.BadRequest) {
            this.completed = false;
            this.enrolmentError = true;
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
