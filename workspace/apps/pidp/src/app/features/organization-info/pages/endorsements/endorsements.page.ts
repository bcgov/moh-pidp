import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';

import { EMPTY, Observable, switchMap } from 'rxjs';

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
  public endorsementRequests$!: Observable<EndorsementRequest[] | null>;
  public endorsements$!: Observable<Endorsement[] | null>;

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
    this.endorsementRequests$ = this.resource
      .approveEndorsementRequest(this.partyService.partyId, requestId)
      .pipe(
        switchMap(() =>
          this.resource.getEndorsementRequests(this.partyService.partyId)
        )
      );
  }

  public onCancel(requestId: number): void {
    this.endorsementRequests$ = this.resource
      .declineEndorsementRequest(this.partyService.partyId, requestId)
      .pipe(
        switchMap(() =>
          this.resource.getEndorsementRequests(this.partyService.partyId)
        )
      );
  }

  // public onCancelEndorsement(endorsementId: number): void {}

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

  protected performSubmission(): NoContent {
    const partyId = this.partyService.partyId;

    return partyId && this.formState.json
      ? this.resource.createEndorsementRequest(partyId, this.formState.json)
      : EMPTY;
  }

  protected afterSubmitIsSuccessful(): void {
    this.navigateToRoot();
  }

  private navigateToRoot(): void {
    this.router.navigate([this.route.snapshot.data.routes.root]);
  }
}
