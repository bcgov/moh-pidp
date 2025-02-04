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
  noop,
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
import { BreadcrumbComponent } from '@app/shared/components/breadcrumb/breadcrumb.component';

import { EndorsementCardComponent } from './components/endorsement-card/endorsement-card.component';
import {
  endorsementRequestsLabelText,
  endorsementRequestsRedStatus,
} from './constants/endorsements.constants';
import { EndorsementsFormState } from './endorsements-form-state';
import { EndorsementsResource } from './endorsements-resource.service';
import { EndorsementRequestStatus } from './enums/endorsement-request-status.enum';
import { EndorsementEmailSearch } from './models/endorsement-email-search.model';
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
    BreadcrumbComponent,
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
  public isDialogOpen = false;

  public constructor(
    dependenciesService: AbstractFormDependenciesService,
    private readonly route: ActivatedRoute,
    private readonly partyService: PartyService,
    private readonly resource: EndorsementsResource,
    private readonly logger: LoggerService,
    private readonly navigationService: NavigationService,
    private readonly lookupService: LookupService,
    viewportService: ViewportService,
    private readonly utilsService: UtilsService,
    fb: FormBuilder,
    private readonly loadingOverlayService: LoadingOverlayService,
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
  public breadcrumbsData: Array<{ title: string; path: string }> = [
    { title: 'Home', path: '' },
    { title: 'Endorsements', path: '' },
  ];
  public popupData: DialogOptions = {
    title: 'Endorsement requests',
    bottomBorder: false,
    titlePosition: 'center',
    bodyTextPosition: 'center',
    component: HtmlComponent,
    data: {
      content:
        'You are about to <b>cancel</b> this endorsement, would you like to proceed',
    },
    imageSrc: '/assets/images/online-marketing-hIgeoQjS_iE-unsplash.jpg',
    imageType: 'banner',
    width: '31rem',
    height: '24rem',
    actionText: 'Continue',
    actionTypePosition: 'center',
    class: 'dialog-container',
  };

  public onEnter(event: Event): void {
    event.preventDefault();
    if (!this.isDialogOpen) {
      const sendButton = document.querySelector(
        'button[type="submit"]',
      ) as HTMLButtonElement;
      if (sendButton) {
        sendButton.click();
      }
    }
  }

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

  public onApprove(requestId: number): void {
    this.loadingOverlayService.open(LOADING_OVERLAY_DEFAULT_MESSAGE);
    const data: DialogOptions = this.popupData;

    data.data = {
      content:
        'You are about to <b>approve</b> this endorsement, would you like to proceed',
    };

    this.isDialogOpen = true;

    this.dialog
      .open(ConfirmDialogComponent, { data })
      .afterClosed()
      .pipe(
        tap(() => (this.isDialogOpen = false)),
        exhaustMap((result) => {
          this.loadingOverlayService.close();
          return result
            ? this.resource
                .approveEndorsementRequest(this.partyService.partyId, requestId)
                .pipe(
                  switchMap(
                    () =>
                      (this.nonActionableEndorsementRequests$ =
                        this.getNonActionableEndorsementRequests(
                          this.partyService.partyId,
                        )),
                  ),
                  switchMap(
                    () =>
                      (this.actionableEndorsementRequests$ =
                        this.getActionableEndorsementRequests(
                          this.partyService.partyId,
                        )),
                  ),
                )
            : EMPTY;
        }),
      )
      .subscribe();
  }

  public onCancel(requestId: number): void {
    const data: DialogOptions = this.popupData;
    data.data = {
      content:
        'You are about to <b>cancel</b> this endorsement, would you like to proceed',
    };
    this.isDialogOpen = true;
    this.dialog
      .open(ConfirmDialogComponent, { data })
      .afterClosed()
      .pipe(
        tap(() => (this.isDialogOpen = false)),
        exhaustMap((result) => {
          this.loadingOverlayService.close();
          return result
            ? this.resource
                .declineEndorsementRequest(this.partyService.partyId, requestId)
                .pipe(
                  switchMap(
                    () =>
                      (this.actionableEndorsementRequests$ =
                        this.getActionableEndorsementRequests(
                          this.partyService.partyId,
                        )),
                  ),
                )
            : EMPTY;
        }),
      )
      .subscribe();
  }

  public onCancelEndorsement(endorsementId: number): void {
    const data: DialogOptions = this.popupData;
    data.data = {
      content:
        'You are about to <b>cancel</b> this endorsement, would you like to proceed',
    };
    this.isDialogOpen = true;
    this.dialog
      .open(ConfirmDialogComponent, { data })
      .afterClosed()
      .pipe(
        tap(() => (this.isDialogOpen = false)),
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

  public getStatus(endorsementRequestStatus: EndorsementRequestStatus): string {
    return (
      endorsementRequestsLabelText[endorsementRequestStatus] ?? 'Requested'
    );
  }

  public isEndorsementRequested(
    endorsementRequestStatus: EndorsementRequestStatus,
  ): boolean {
    return !endorsementRequestsRedStatus.includes(endorsementRequestStatus);
  }

  public getCollegeText(
    endorsementCard: EndorsementRequest | Endorsement,
  ): string {
    const college = this.lookupService.colleges.find(
      (x) => x.code === endorsementCard.collegeCode,
    );
    return college?.name ?? '';
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

  protected performSubmission(): NoContent {
    const partyId = this.partyService.partyId;

    const data: DialogOptions = {
      title: 'Endorsement requests',
      bottomBorder: false,
      titlePosition: 'center',
      bodyTextPosition: 'center',
      component: HtmlComponent,
      data: {
        content: '',
      },
      imageSrc: '/assets/images/online-marketing-hIgeoQjS_iE-unsplash.jpg',
      imageType: 'banner',
      width: '31rem',
      height: '26rem',
      actionText: 'Continue',
      actionTypePosition: 'center',
      class: 'dialog-container',
    };

    this.isDialogOpen = true;
    return partyId && this.formState.json
      ? this.resource
          .emailSearch(partyId, this.formState.json.recipientEmail)
          .pipe(
            switchMap((response: EndorsementEmailSearch) => {
              data.data!.content = response.recipientName
                ? `An existing user has registered ${this.formState.json?.recipientEmail}. Please confirm you are wanting an endorsement with <p class='p-0 m-0' style='color: #036;font-size:1.2rem;'><b>${response.recipientName}</b></p>`
                : `You are about to <b>request</b> an endorsement to<p class='p-0 m-0' style='color: #036;font-size:1.2rem;'><b>${this.formState.json?.recipientEmail}</b></p>would you like to proceed?`;

              return this.dialog
                .open(ConfirmDialogComponent, { data })
                .afterClosed()
                .pipe(
                  tap(() => (this.isDialogOpen = false)),
                  exhaustMap((result) => {
                    this.loadingOverlayService.close();
                    return result && partyId && this.formState.json
                      ? this.resource.createEndorsementRequest(partyId, {
                          ...this.formState.json,
                          preApproved: response.recipientName ? true : false,
                        })
                      : EMPTY;
                  }),
                  catchError((error: HttpErrorResponse) => {
                    this.loadingOverlayService.close();
                    if (error.status === HttpStatusCode.BadRequest) {
                      return of(noop());
                    }
                    return of(noop());
                  }),
                );
            }),
          )
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
}
