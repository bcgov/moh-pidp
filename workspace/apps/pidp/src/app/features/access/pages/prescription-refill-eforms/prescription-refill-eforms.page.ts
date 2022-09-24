import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { catchError, noop, of, tap } from 'rxjs';

import { PartyService } from '@app/core/party/party.service';
import { LoggerService } from '@app/core/services/logger.service';
import { StatusCode } from '@app/features/portal/enums/status-code.enum';

import { PrescriptionRefillEformsResource } from './prescription-refill-eforms-resource.service';

@Component({
  selector: 'app-prescription-refill-eforms',
  templateUrl: './prescription-refill-eforms.page.html',
  styleUrls: ['./prescription-refill-eforms.page.scss'],
})
export class PrescriptionRefillEformsPage implements OnInit {
  public title: string;
  public completed: boolean | null;
  public accessRequestFailed: boolean;
  public enrolmentError: boolean;

  public constructor(
    private route: ActivatedRoute,
    private router: Router,
    private partyService: PartyService,
    private resource: PrescriptionRefillEformsResource,
    private logger: LoggerService
  ) {
    const routeData = this.route.snapshot.data;
    this.title = routeData.title;
    this.completed = routeData.uciStatusCode === StatusCode.COMPLETED;
    this.accessRequestFailed = false;
    this.enrolmentError = false;
  }

  public onBack(): void {
    this.navigateToRoot();
  }

  public onRequestAccess(): void {
    this.resource
      .requestAccess(this.partyService.partyId)
      .pipe(
        tap(() => (this.completed = true)),
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

  private navigateToRoot(): void {
    this.router.navigate([this.route.snapshot.data.routes.root]);
  }
}
