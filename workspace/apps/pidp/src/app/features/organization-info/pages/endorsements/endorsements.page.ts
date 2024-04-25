import { AsyncPipe, DatePipe, NgFor, NgIf } from '@angular/common';
import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';
import { Component, OnInit, ViewChild } from '@angular/core';
import {
  FormBuilder,
  FormControl,
  FormGroupDirective,
  ReactiveFormsModule,
} from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatTabsModule } from '@angular/material/tabs';
import { ActivatedRoute } from '@angular/router';

import {
  EMPTY,
  Observable,
  catchError,
  exhaustMap,
  map,
  of,
  switchMap,
  tap,
} from 'rxjs';

import { FaIconComponent } from '@fortawesome/angular-fontawesome';
import {
  faArrowDown,
  faArrowUp,
  faUser,
  faUserGroup,
} from '@fortawesome/free-solid-svg-icons';
import {
  LOADING_OVERLAY_DEFAULT_MESSAGE,
  LoadingOverlayService,
  NavigationService,
} from '@pidp/presentation';

import { NoContent } from '@bcgov/shared/data-access';
import {
  ConfirmDialogComponent,
  DialogOptions,
  HtmlComponent,
  InjectViewportCssClassDirective,
  PageFooterActionDirective,
  PidpViewport,
  ViewportService,
} from '@bcgov/shared/ui';

import {
  AbstractFormDependenciesService,
  AbstractFormPage,
} from '@app/core/classes/abstract-form-page.class';
import { PartyService } from '@app/core/party/party.service';
import { LoggerService } from '@app/core/services/logger.service';
import { UtilsService } from '@app/core/services/utils.service';
import { StatusCode } from '@app/features/portal/enums/status-code.enum';
import { LookupService } from '@app/modules/lookup/lookup.service';

import { EndorsementCardComponent } from './components/endorsement-card/endorsement-card.component';
import { EndorsementsFormState } from './endorsements-form-state';
import { EndorsementsResource } from './endorsements-resource.service';
import { EndorsementRequestStatus } from './enums/endorsement-request-status.enum';
import { EndorsementRequest } from './models/endorsement-request.model';
import { Endorsement } from './models/endorsement.model';

export enum EndorsementType {
  WorkingRelationship,
  IncomingRequest,
  OutgoingRequest,
}
@Component({
  selector: 'app-endorsements',
  templateUrl: './endorsements.page.html',
  styleUrls: ['./endorsements.page.scss'],
  standalone: true,
  imports: [
    AsyncPipe,
    DatePipe,
    EndorsementCardComponent,
    FaIconComponent,
    InjectViewportCssClassDirective,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    MatTabsModule,
    NgFor,
    NgIf,
    PageFooterActionDirective,
    ReactiveFormsModule,
  ],
})
export class EndorsementsPage
  extends AbstractFormPage<EndorsementsFormState>
  implements OnInit
{
  @ViewChild(FormGroupDirective) public formGroupDirective!: FormGroupDirective;

  public faUser = faUser;
  public faUserGroup = faUserGroup;
  public faArrowUp = faArrowUp;
  public faArrowDown = faArrowDown;

  public formState: EndorsementsFormState;
  public completed: boolean | null;
  public actionableEndorsementRequests$!: Observable<EndorsementRequest[]>;
  public nonActionableEndorsementRequests$!: Observable<EndorsementRequest[]>;
  public endorsements$!: Observable<Endorsement[]>;

  public showOverlayOnSubmit = true;

  public constructor(
    dependenciesService: AbstractFormDependenciesService,
    private route: ActivatedRoute,
    private partyService: PartyService,
    private resource: EndorsementsResource,
    private logger: LoggerService,
    private navigationService: NavigationService,
    private lookupService: LookupService,
    viewportService: ViewportService,
    private utilsService: UtilsService,
    fb: FormBuilder,
    private loadingOverlayService: LoadingOverlayService,
  ) {
    super(dependenciesService);

    const routeData = this.route.snapshot.data;
    this.formState = new EndorsementsFormState(fb);
    this.completed =
      routeData.endorsementRequestStatusCode === StatusCode.COMPLETED;
    viewportService.viewportBroadcast$.subscribe((viewport) =>
      this.onViewportChange(viewport),
    );
  }

  public showTextLabels = false;
  public showIconLabels = true;

  public get recipientEmail(): FormControl {
    return this.formState.form.get('recipientEmail') as FormControl;
  }

  private onViewportChange(viewport: PidpViewport): void {
    if (viewport === PidpViewport.xsmall) {
      this.showIconLabels = true;
      this.showTextLabels = false;
    } else {
      this.showIconLabels = false;
      this.showTextLabels = true;
    }
  }

  public onBack(): void {
    this.navigationService.navigateToRoot();
  }

  public onApprove(requestId: number): void {
    this.loadingOverlayService.open(LOADING_OVERLAY_DEFAULT_MESSAGE);
    this.resource
      .approveEndorsementRequest(this.partyService.partyId, requestId)
      .pipe(
        switchMap(
          () =>
            (this.actionableEndorsementRequests$ =
              this.getActionableEndorsementRequests(this.partyService.partyId)),
        ),
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
              this.getActionableEndorsementRequests(this.partyService.partyId)),
        ),
      )
      .subscribe();
  }

  public onCancelEndorsement(endorsementId: number): void {
    const data: DialogOptions = {
      title: 'Cancel Endorsement',
      component: HtmlComponent,
      data: {
        content: 'Are you sure you want to cancel this Endorsement?',
      },
    };
    this.dialog
      .open(ConfirmDialogComponent, { data })
      .afterClosed()
      .pipe(
        exhaustMap((result) =>
          result
            ? this.resource
                .cancelEndorsement(this.partyService.partyId, endorsementId)
                .pipe(
                  switchMap(
                    () =>
                      (this.endorsements$ = this.getEndorsements(
                        this.partyService.partyId,
                      )),
                  ),
                )
            : EMPTY,
        ),
      )
      .subscribe();
  }

  public ngOnInit(): void {
    this.utilsService.scrollTop();
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

  public getCollegeTextForEndorsement(endorsement: Endorsement): string {
    const college = this.lookupService.colleges.find(
      (x) => x.code === endorsement.collegeCode,
    );
    return college?.name ?? '';
  }

  public getCollegeTextForEndorsementRequest(
    endorsementRequest: EndorsementRequest,
  ): string {
    const college = this.lookupService.colleges.find(
      (x) => x.code === endorsementRequest.collegeCode,
    );
    return college?.name ?? '';
  }

  protected performSubmission(): NoContent {
    const partyId = this.partyService.partyId;

    return partyId && this.formState.json
      ? this.resource.createEndorsementRequest(partyId, this.formState.json)
      : EMPTY;
  }

  protected afterSubmitIsSuccessful(): void {
    this.formGroupDirective.resetForm();

    this.nonActionableEndorsementRequests$ =
      this.getNonActionableEndorsementRequests(this.partyService.partyId);
  }

  private navigateToRoot(): void {
    this.navigationService.navigateToRoot();
  }

  private getEndorsements(partyId: number): Observable<Endorsement[]> {
    return this.resource.getEndorsements(partyId).pipe(
      map((response: Endorsement[] | null) => response ?? []),
      catchError((error: HttpErrorResponse) => {
        if (error.status === HttpStatusCode.NotFound) {
          this.navigateToRoot();
        }
        return of([]);
      }),
    );
  }

  private getActionableEndorsementRequests(
    partyId: number,
  ): Observable<EndorsementRequest[]> {
    return this.resource.getEndorsementRequests(partyId).pipe(
      map((response: EndorsementRequest[] | null) => response ?? []),
      tap((_) => this.loadingOverlayService.close()),
      map((response: EndorsementRequest[]) =>
        response.filter((res) => res.actionable === true),
      ),
      catchError((error: HttpErrorResponse) => {
        if (error.status === HttpStatusCode.NotFound) {
          this.navigateToRoot();
        }
        return of([]);
      }),
    );
  }

  private getNonActionableEndorsementRequests(
    partyId: number,
  ): Observable<EndorsementRequest[]> {
    return this.resource.getEndorsementRequests(partyId).pipe(
      map((response: EndorsementRequest[] | null) => response ?? []),
      map((response: EndorsementRequest[]) =>
        response.filter((res) => res.actionable === false),
      ),
      catchError((error: HttpErrorResponse) => {
        if (error.status === HttpStatusCode.NotFound) {
          this.navigateToRoot();
        }
        return of([]);
      }),
    );
  }

  public GetStatus(endorsementRequestStatus: EndorsementRequestStatus): string {
    let statusText = 'Requested';
    if (endorsementRequestStatus === EndorsementRequestStatus.CANCELLED) {
      statusText = 'Cancelled';
    } else if (endorsementRequestStatus === EndorsementRequestStatus.DECLINED) {
      statusText = 'Declined';
    } else if (endorsementRequestStatus === EndorsementRequestStatus.APPROVED) {
      statusText = 'In progress';
    }
    return statusText;
  }

  public IsEndorsementRequested(
    endorsementRequestStatus: EndorsementRequestStatus,
  ): boolean {
    let endorsementRequested = true;
    if (
      endorsementRequestStatus === EndorsementRequestStatus.CANCELLED ||
      endorsementRequestStatus === EndorsementRequestStatus.DECLINED
    ) {
      endorsementRequested = false;
    }
    return endorsementRequested;
  }
}
