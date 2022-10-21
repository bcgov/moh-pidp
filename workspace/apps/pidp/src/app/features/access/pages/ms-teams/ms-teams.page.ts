import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';

import { EMPTY, Observable, catchError, forkJoin, noop, of, tap } from 'rxjs';

import { NoContent } from '@bcgov/shared/data-access';

import { AbstractFormPage } from '@app/core/classes/abstract-form-page.class';
import { PartyService } from '@app/core/party/party.service';
import { DocumentService } from '@app/core/services/document.service';
import { FormUtilsService } from '@app/core/services/form-utils.service';
import { LoggerService } from '@app/core/services/logger.service';
import { UtilsService } from '@app/core/services/utils.service';
import { User } from '@app/features/auth/models/user.model';
import { AuthorizedUserService } from '@app/features/auth/services/authorized-user.service';
import { StatusCode } from '@app/features/portal/enums/status-code.enum';
import { PersonalInformationResource } from '@app/features/profile/pages/personal-information/personal-information-resource.service';

import { MsTeamsFormState } from './ms-teams-form-state';
import { MsTeamsResource } from './ms-teams-resource.service';
import { msTeamsSupportEmail } from './ms-teams.constants';
import { ClinicMember } from './ms-teams.model';

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
  public user$: Observable<User>;

  public constructor(
    protected dialog: MatDialog,
    protected formUtilsService: FormUtilsService,
    private route: ActivatedRoute,
    private router: Router,
    private partyService: PartyService,
    private resource: MsTeamsResource,
    private authorizedUserService: AuthorizedUserService,
    private logger: LoggerService,
    private personalInformationResource: PersonalInformationResource,
    private utilsService: UtilsService,
    private documentService: DocumentService,
    fb: FormBuilder
  ) {
    super(dialog, formUtilsService);
    const routeData = this.route.snapshot.data;
    this.completed = routeData.msTeamsStatusCode === StatusCode.COMPLETED;
    this.msTeamsSupportEmail = msTeamsSupportEmail;
    this.currentPage = 0;
    this.enrolmentError = false;
    this.submissionPage = documentService.getMsTeamsAgreementPageCount() + 1;
    this.formState = new MsTeamsFormState(fb, formUtilsService);
    this.user$ = this.authorizedUserService.user$;
  }

  public onBack(): void {
    if (this.currentPage === 0 || this.completed) {
      this.navigateToRoot();
    } else {
      this.utilsService.scrollTop('.mat-sidenav-content');
      this.currentPage--;
    }
  }

  public onNext(): void {
    if (this.currentPage === 0 && !this.validateFirstPage()) {
      return;
    }

    this.utilsService.scrollTop('.mat-sidenav-content');
    this.currentPage++;
  }

  public addClinicMember(): void {
    const member = this.formState.buildClinicMemberForm();
    this.formState.clinicMembers.push(member);
  }

  public removeClinicMember(index: number): void {
    this.formState.clinicMembers.removeAt(index);
  }

  public getAgreementText(page: number): string {
    return this.documentService.getMsTeamsAgreement(page);
  }

  public prepopulateForm(): void {
    const partyId = this.partyService.partyId;

    forkJoin({
      personalInfo: this.personalInformationResource.get(partyId),
      user: this.authorizedUserService.user$,
    }).subscribe(({ personalInfo, user }) => {
      const privacyOfficer: ClinicMember = {
        name: `${user.firstName} ${user.lastName}`,
        email: personalInfo?.email ?? '',
        jobTitle: 'Privacy Officer',
        phone: personalInfo?.phone ?? '',
      };

      this.formState.clinicMemberControls[0].setValue(privacyOfficer);
    });
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

    this.prepopulateForm();
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

  private navigateToRoot(): void {
    this.router.navigate([this.route.snapshot.data.routes.root]);
  }
}
