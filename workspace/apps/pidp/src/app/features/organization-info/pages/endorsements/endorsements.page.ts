import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';

import { EMPTY, Observable, catchError, map, of, switchMap } from 'rxjs';

import { NoContent } from '@bcgov/shared/data-access';

import { AbstractFormPage } from '@app/core/classes/abstract-form-page.class';
import { PartyService } from '@app/core/party/party.service';
import { FormUtilsService } from '@app/core/services/form-utils.service';
import { LoggerService } from '@app/core/services/logger.service';
import { StatusCode } from '@app/features/portal/enums/status-code.enum';

import { EndorsementsFormState } from './endorsements-form-state';
import { EndorsementsResource } from './endorsements-resource.service';
import { EndorsementRequest } from './models/endorsement-request.model';
import { Endorsement } from './models/endorsement.model';

@Component({
  selector: 'app-endorsements',
  templateUrl: './endorsements.page.html',
  styleUrls: ['./endorsements.page.scss'],
})
export class EndorsementsPage
  extends AbstractFormPage<EndorsementsFormState>
  implements OnInit
{
  public title: string;
  public formState: EndorsementsFormState;
  public completed: boolean | null;
  public actionableEndorsementRequests$!: Observable<EndorsementRequest[]>;
  public nonActionableEndorsementRequests$!: Observable<EndorsementRequest[]>;
  public endorsements$!: Observable<Endorsement[]>;

  public constructor(
    protected dialog: MatDialog,
    protected formUtilsService: FormUtilsService,
    private route: ActivatedRoute,
    private router: Router,
    private partyService: PartyService,
    private resource: EndorsementsResource,
    private logger: LoggerService,
    fb: FormBuilder
  ) {
    super(dialog, formUtilsService);

    const routeData = this.route.snapshot.data;
    this.title = routeData.title;
    this.formState = new EndorsementsFormState(fb);
    this.completed =
      routeData.endorsementRequestStatusCode === StatusCode.COMPLETED;
  }
  public onBack(): void {
    this.navigateToRoot();
  }

  public onApprove(requestId: number): void {
    this.resource
      .approveEndorsementRequest(this.partyService.partyId, requestId)
      .pipe(
        switchMap(
          () =>
            (this.actionableEndorsementRequests$ =
              this.getActionableEndorsementRequests(this.partyService.partyId))
        )
      )
      .subscribe();
  }

  public onCancel(requestId: number): void {
    this.resource
      .declineEndorsementRequest(this.partyService.partyId, requestId)
      .pipe(
        switchMap(
          () =>
            (this.actionableEndorsementRequests$ =
              this.getActionableEndorsementRequests(this.partyService.partyId))
        )
      )
      .subscribe();
  }

  public onCancelEndorsement(endorsementId: number): void {
    this.resource
      .cancelEndorsement(this.partyService.partyId, endorsementId)
      .pipe(
        switchMap(
          () =>
            (this.endorsements$ = this.getEndorsements(
              this.partyService.partyId
            ))
        )
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

    this.endorsements$ = this.getEndorsements(partyId);

    this.actionableEndorsementRequests$ =
      this.getActionableEndorsementRequests(partyId);

    this.nonActionableEndorsementRequests$ =
      this.getNonActionableEndorsementRequests(partyId);
  }

  protected performSubmission(): NoContent {
    const partyId = this.partyService.partyId;

    return partyId && this.formState.json
      ? this.resource.createEndorsementRequest(partyId, this.formState.json)
      : EMPTY;
  }

  protected afterSubmitIsSuccessful(): void {
    this.formState.form.reset();
    this.formState.form.clearValidators();

    this.nonActionableEndorsementRequests$ =
      this.getNonActionableEndorsementRequests(this.partyService.partyId);
  }

  private navigateToRoot(): void {
    this.router.navigate([this.route.snapshot.data.routes.root]);
  }

  private getEndorsements(partyId: number): Observable<Endorsement[]> {
    return this.resource.getEndorsements(partyId).pipe(
      map((response: Endorsement[] | null) => response ?? []),
      catchError((error: HttpErrorResponse) => {
        if (error.status === HttpStatusCode.NotFound) {
          this.navigateToRoot();
        }
        return of([]);
      })
    );
  }

  private getActionableEndorsementRequests(
    partyId: number
  ): Observable<EndorsementRequest[]> {
    return this.resource.getEndorsementRequests(partyId).pipe(
      map((response: EndorsementRequest[] | null) => response ?? []),
      map((response: EndorsementRequest[]) =>
        response.filter((res) => res.actionable === true)
      ),
      catchError((error: HttpErrorResponse) => {
        if (error.status === HttpStatusCode.NotFound) {
          this.navigateToRoot();
        }
        return of([]);
      })
    );
  }

  private getNonActionableEndorsementRequests(
    partyId: number
  ): Observable<EndorsementRequest[]> {
    return this.resource.getEndorsementRequests(partyId).pipe(
      map((response: EndorsementRequest[] | null) => response ?? []),
      map((response: EndorsementRequest[]) =>
        response.filter((res) => res.actionable === false)
      ),
      catchError((error: HttpErrorResponse) => {
        if (error.status === HttpStatusCode.NotFound) {
          this.navigateToRoot();
        }
        return of([]);
      })
    );
  }
}
