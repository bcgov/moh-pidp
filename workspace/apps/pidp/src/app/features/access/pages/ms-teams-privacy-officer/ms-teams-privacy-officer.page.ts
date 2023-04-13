import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import { EMPTY, catchError, noop, of, tap } from 'rxjs';

import { LoadingOverlayService } from '@pidp/presentation';

import { NoContent } from '@bcgov/shared/data-access';

import {
  AbstractFormDependenciesService,
  AbstractFormPage,
} from '@app/core/classes/abstract-form-page.class';
import { PartyService } from '@app/core/party/party.service';
import { DocumentService } from '@app/core/services/document.service';
import { LoggerService } from '@app/core/services/logger.service';
import { UtilsService } from '@app/core/services/utils.service';
import { StatusCode } from '@app/features/portal/enums/status-code.enum';

import { MsTeamsPrivacyOfficerFormState } from './ms-teams-privacy-officer-form-state';
import { MsTeamsPrivacyOfficerResource } from './ms-teams-privacy-officer-resource.service';
import { msTeamsSupportEmail } from './ms-teams.constants';

@Component({
  selector: 'app-ms-teams',
  templateUrl: './ms-teams-privacy-officer.page.html',
  styleUrls: ['./ms-teams-privacy-officer.page.scss'],
})
export class MsTeamsPrivacyOfficerPage
  extends AbstractFormPage<MsTeamsPrivacyOfficerFormState>
  implements OnInit
{
  public completed: boolean | null;
  public msTeamsSupportEmail: string;
  public currentPage: number;
  public enrolmentError: boolean;
  public submissionPage: number;
  public formState: MsTeamsPrivacyOfficerFormState;

  // ui-page is handling this.
  public showOverlayOnSubmit = false;

  public constructor(
    dependenciesService: AbstractFormDependenciesService,
    private route: ActivatedRoute,
    private router: Router,
    private partyService: PartyService,
    private resource: MsTeamsPrivacyOfficerResource,
    private logger: LoggerService,
    private utilsService: UtilsService,
    private documentService: DocumentService,
    fb: FormBuilder,
    private loadingOverlayService: LoadingOverlayService
  ) {
    super(dependenciesService);
    const routeData = this.route.snapshot.data;
    this.completed =
      routeData.msTeamsPrivacyOfficerStatusCode === StatusCode.COMPLETED;
    this.msTeamsSupportEmail = msTeamsSupportEmail;
    this.currentPage = 0;
    this.enrolmentError = false;
    this.submissionPage = documentService.getMsTeamsAgreementPageCount();
    this.formState = new MsTeamsPrivacyOfficerFormState(
      fb,
      dependenciesService.formUtilsService
    );
  }

  public onBack(): void {
    if (this.currentPage === 0 || this.completed) {
      this.navigateToRoot();
    } else {
      this.utilsService.scrollTopWithDelay();
      this.currentPage--;
    }
  }

  public onNext(): void {
    if (this.currentPage === 0 && !this.validateFirstPage()) {
      return;
    }

    this.utilsService.scrollTopWithDelay();
    this.currentPage++;
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
  }

  protected performSubmission(): NoContent {
    const partyId = this.partyService.partyId;

    this.loadingOverlayService.open();

    return partyId && this.formState.json
      ? this.resource
          .requestAccess(this.partyService.partyId, this.formState.json)
          .pipe(
            tap(() => {
              this.loadingOverlayService.close();
              this.completed = true;
              this.enrolmentError = false;
            }),
            catchError((error: HttpErrorResponse) => {
              this.loadingOverlayService.close();
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

  private navigateToRoot(): void {
    this.router.navigate([this.route.snapshot.data.routes.root]);
  }
}
