import { AsyncPipe, NgIf } from '@angular/common';
import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';
import { Component, OnInit, forwardRef } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { ActivatedRoute, Router } from '@angular/router';

import { Observable, catchError, noop, of, tap } from 'rxjs';

import {
  AlertComponent,
  AlertContentDirective,
  AnchorDirective,
  PageComponent,
  PageFooterActionDirective,
  PageFooterComponent,
} from '@bcgov/shared/ui';

import { PartyService } from '@app/core/party/party.service';
import { LoggerService } from '@app/core/services/logger.service';
import { UtilsService } from '@app/core/services/utils.service';
import { specialAuthorityEformsSupportEmail } from '@app/features/access/pages/sa-eforms/sa-eforms.constants';
import { StatusCode } from '@app/features/portal/enums/status-code.enum';
import {
  DashboardStateModel,
  PidpStateName,
} from '@app/features/portal/models/state.model';
import { AppStateService } from '@app/features/shell/services/app-state.service';

import { UserAccessAgreementDocumentComponent } from './components/user-access-agreement-document/user-access-agreement-document.component';
import { UserAccessAgreementResource } from './user-access-agreement-resource.service';

@Component({
  selector: 'app-user-access-agreement',
  templateUrl: './user-access-agreement.page.html',
  styleUrls: ['./user-access-agreement.page.scss'],
  standalone: true,
  imports: [
    AlertComponent,
    AlertContentDirective,
    AnchorDirective,
    AsyncPipe,
    forwardRef(() => UserAccessAgreementDocumentComponent),
    MatButtonModule,
    NgIf,
    PageComponent,
    PageFooterActionDirective,
    PageFooterComponent,
  ],
})
export class UserAccessAgreementPage implements OnInit {
  public title: string;
  public completed: boolean | null;
  public accessRequestFailed: boolean;
  public specialAuthoritySupportEmail: string;
  public redirectUrl: string | null = null;
  public dashboardState$: Observable<DashboardStateModel>;

  public constructor(
    private route: ActivatedRoute,
    private router: Router,
    private partyService: PartyService,
    private resource: UserAccessAgreementResource,
    private logger: LoggerService,
    private utilsService: UtilsService,
    private stateService: AppStateService,
  ) {
    this.title = this.route.snapshot.data.title;
    this.dashboardState$ = this.stateService.getNamedStateBroadcast(
      PidpStateName.dashboard,
    );

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
        }),
      )
      .subscribe();
  }

  public onBack(): void {
    this.navigateToRoot();
    this.utilsService.scrollTop();
  }

  private navigateToRoot(): void {
    this.router.navigate([this.route.snapshot.data.routes.root]);
  }
}
