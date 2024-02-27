import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { Observable } from 'rxjs';

import { DashboardStateModel, PidpStateName } from '@pidp/data-model';
import { AppStateService } from '@pidp/presentation';

import { PartyService } from '@app/core/party/party.service';
import { LoggerService } from '@app/core/services/logger.service';
import { UtilsService } from '@app/core/services/utils.service';
import { StatusCode } from '@app/features/portal/enums/status-code.enum';

@Component({
  selector: 'app-account-linking',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './account-linking.page.html',
  styleUrl: './account-linking.page.scss',
})
export class AccountLinkingPage implements OnInit {
  public title: string;
  public completed: boolean | null;
  public redirectUrl: string | null = null;
  public dashboardState$: Observable<DashboardStateModel>;

  public constructor(
    private route: ActivatedRoute,
    private router: Router,
    private partyService: PartyService,
    private logger: LoggerService,
    private utilsService: UtilsService,
    private stateService: AppStateService,
  ) {
    this.title = this.route.snapshot.data.title;
    this.dashboardState$ = this.stateService.getNamedStateBroadcast(
      PidpStateName.dashboard,
    );

    const routeData = this.route.snapshot.data;
    this.completed = routeData.accountLinking === StatusCode.COMPLETED;
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

  public onLinkAccount(): void {
    // this.resource
    //   .acceptAgreement(this.partyService.partyId)
    //   .pipe(
    //     tap(() => {
    //       this.completed = true;
    //       if (!this.redirectUrl) {
    //         this.navigateToRoot();
    //       } else {
    //         this.router.navigate([this.redirectUrl]);
    //       }
    //       this.utilsService.scrollTopWithDelay();
    //     }),
    //     catchError((error: HttpErrorResponse) => {
    //       if (error.status === HttpStatusCode.BadRequest) {
    //         this.completed = false;
    //         return of(noop());
    //       }
    //       this.accessRequestFailed = true;
    //       return of(noop());
    //     }),
    //   )
    //   .subscribe();
  }

  public onBack(): void {
    this.navigateToRoot();
  }

  private navigateToRoot(): void {
    this.router.navigate([this.route.snapshot.data.routes.root]);
  }
}
