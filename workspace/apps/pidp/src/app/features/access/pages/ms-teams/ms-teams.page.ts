import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, ParamMap, Router } from '@angular/router';

import { EMPTY, catchError, noop, of, tap } from 'rxjs';

import { ScrollToTopService } from '@pidp/presentation';

import { NoContent } from '@bcgov/shared/data-access';

import { AbstractFormPage } from '@app/core/classes/abstract-form-page.class';
import { PartyService } from '@app/core/party/party.service';
import { DocumentService } from '@app/core/services/document.service';
import { FormUtilsService } from '@app/core/services/form-utils.service';
import { LoggerService } from '@app/core/services/logger.service';
import { StatusCode } from '@app/features/portal/enums/status-code.enum';

import { MsTeamsFormState } from './ms-teams-form-state';
import { MsTeamsResource } from './ms-teams-resource.service';
import { msTeamsSupportEmail } from './ms-teams.constants';

@Component({
  selector: 'app-ms-teams',
  templateUrl: './ms-teams.page.html',
  styleUrls: ['./ms-teams.page.scss'],
})
export class MsTeamsPage
  extends AbstractFormPage<MsTeamsFormState>
  implements OnInit
{
  public completed: boolean | null;
  public msTeamsSupportEmail: string;
  public currentPage: number;
  public enrolmentError: boolean;
  public submissionPage: number;
  public formState: MsTeamsFormState;

  public constructor(
    protected dialog: MatDialog,
    protected formUtilsService: FormUtilsService,
    private route: ActivatedRoute,
    private router: Router,
    private partyService: PartyService,
    private resource: MsTeamsResource,
    private logger: LoggerService,
    private documentService: DocumentService,
    fb: FormBuilder,
    public scrollToTopService: ScrollToTopService
  ) {
    super(dialog, formUtilsService);
    const routeData = this.route.snapshot.data;
    this.completed = routeData.msTeamsStatusCode === StatusCode.COMPLETED;
    this.msTeamsSupportEmail = msTeamsSupportEmail;
    this.currentPage = 0;
    this.enrolmentError = false;
    this.submissionPage = documentService.getMsTeamsAgreementPageCount();
    this.formState = new MsTeamsFormState(fb, formUtilsService);

    // Ensure the pageid passed by the url is equal to currentPage.
    // If it does not match, redirect to the correct page.
    // NOTE: The pageid is passed in the url in order to allow each screen of text to have its own url,
    //       while keeping all the code in a single component. "Scroll to top" seems to work better when
    //       each different page of text has its own url, as opposed to a single url that dynamically
    //       displays different text depending on page state.
    //       The redirect here is required to ensure the user cannot skip pages in the workflow.
    this.route.paramMap.subscribe((params: ParamMap) => {
      const pageId = parseInt(params.get('pageid') ?? '0');
      if (pageId !== this.currentPage) {
        this.navigateToPage(this.currentPage);
        return;
      }
    });
  }

  public onBack(): void {
    if (this.currentPage === 0 || this.completed) {
      this.navigateToRoot();
    } else {
      this.currentPage--;
      this.navigateToPage(this.currentPage);
    }
  }

  public onNext(): void {
    if (this.currentPage === 0 && !this.validateFirstPage()) {
      return;
    }

    this.currentPage++;
    this.navigateToPage(this.currentPage);
  }

  public getAgreementText(page: number): string {
    return this.documentService.getMsTeamsAgreement(page);
  }
  public ngOnInit(): void {
    if (!this.partyService.partyId) {
      this.logger.error('No party ID was provided');
      return this.navigateToRoot();
    }

    if (this.completed === null) {
      this.logger.error('No status code was provided');
      return this.navigateToRoot();
    }

    this.scrollToTopService.configure();
  }

  protected performSubmission(): NoContent {
    const partyId = this.partyService.partyId;

    return partyId && this.formState.json
      ? this.resource
          .requestAccess(this.partyService.partyId, this.formState.json)
          .pipe(
            tap(() => (this.completed = true)),
            catchError((error: HttpErrorResponse) => {
              if (error.status === HttpStatusCode.BadRequest) {
                this.completed = false;
                this.enrolmentError = true;
                return of(noop());
              }
              return of(noop());
            })
          )
      : EMPTY;
  }

  private validateFirstPage(): boolean {
    return this.checkValidity(
      new FormArray([this.formState.clinicName, this.formState.clinicAddress])
    );
  }

  private navigateToPage(pageNumber: number): void {
    const url = `/access/ms-teams/${pageNumber}`;
    this.router.navigateByUrl(url);
  }
  private navigateToRoot(): void {
    this.router.navigate([this.route.snapshot.data.routes.root]);
  }
}
