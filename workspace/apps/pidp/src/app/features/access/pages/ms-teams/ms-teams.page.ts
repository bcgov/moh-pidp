import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { catchError, noop, of, tap } from 'rxjs';

import { PartyService } from '@app/core/party/party.service';
import { DocumentService } from '@app/core/services/document.service';
import { LoggerService } from '@app/core/services/logger.service';
import { UtilsService } from '@app/core/services/utils.service';
import { StatusCode } from '@app/features/portal/enums/status-code.enum';

import { MsTeamsResource } from './ms-teams-resource.service';
import { msTeamsSupportEmail } from './ms-teams.constants';

@Component({
  selector: 'app-ms-teams',
  templateUrl: './ms-teams.page.html',
  styleUrls: ['./ms-teams.page.scss'],
})
export class MsTeamsPage implements OnInit {
  public completed: boolean | null;
  public declarationAgreement: string;
  public detailsAgreement: string;
  public itSecurityAgreement: string;
  public msTeamsSupportEmail: string;
  public currentPage: number;
  public enrolmentError: boolean;

  public constructor(
    private route: ActivatedRoute,
    private router: Router,
    private partyService: PartyService,
    private resource: MsTeamsResource,
    private logger: LoggerService,
    private utilsService: UtilsService,
    documentService: DocumentService
  ) {
    const routeData = this.route.snapshot.data;
    this.completed = routeData.msTeamsStatusCode === StatusCode.COMPLETED;
    this.declarationAgreement =
      documentService.getMsTeamsDeclarationAgreement();
    this.detailsAgreement = documentService.getMsTeamsDetailsAgreement();
    this.itSecurityAgreement = documentService.getMsTeamsITSecurityAgreement();
    this.msTeamsSupportEmail = msTeamsSupportEmail;
    this.currentPage = 0;
    this.enrolmentError = false;
  }

  public onBack(): void {
    if (this.currentPage > 0) {
      this.utilsService.scrollTop('.mat-sidenav-content');
      this.currentPage--;
    } else {
      this.navigateToRoot();
    }
  }

  public onRequestAccess(): void {
    if (this.currentPage < 2) {
      this.utilsService.scrollTop('.mat-sidenav-content');
      this.currentPage++;
    } else {
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
            return of(noop());
          })
        )
        .subscribe();
    }
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
