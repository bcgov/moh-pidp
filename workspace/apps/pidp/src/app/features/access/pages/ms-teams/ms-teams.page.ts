import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { catchError, noop, of, tap } from 'rxjs';

import { PartyService } from '@app/core/party/party.service';
import { DocumentService } from '@app/core/services/document.service';
import { LoggerService } from '@app/core/services/logger.service';
import { StatusCode } from '@app/features/portal/enums/status-code.enum';

import { MsTeamsResource } from './ms-teams-resource.service';

@Component({
  selector: 'app-ms-teams',
  templateUrl: './ms-teams.page.html',
  styleUrls: ['./ms-teams.page.scss'],
})
export class MsTeamsPage implements OnInit {
  public title: string;
  public completed: boolean | null;
  public declarationAgreement: string;
  public detailsAgreement: string;
  public itSecurityAgreement: string;
  public currentPage: number;
  public constructor(
    private route: ActivatedRoute,
    private router: Router,
    private partyService: PartyService,
    private resource: MsTeamsResource,
    private logger: LoggerService,
    documentService: DocumentService
  ) {
    const routeData = this.route.snapshot.data;
    this.title = routeData.title;
    this.completed = routeData.msTeamsStatusCode === StatusCode.COMPLETED;
    this.declarationAgreement =
      documentService.getMsTeamsDeclarationAgreement();
    this.detailsAgreement = documentService.getMsTeamsDetailsAgreement();
    this.itSecurityAgreement = documentService.getMsTeamsITSecurityAgreement();
    this.currentPage = 0;
  }

  public onBack(): void {
    if (this.currentPage > 0) {
      this.currentPage--;
    } else {
      this.navigateToRoot();
    }
  }

  public onRequestAccess(): void {
    if (this.currentPage < 2) {
      this.currentPage++;
    } else {
      this.resource
        .requestAccess(this.partyService.partyId)
        .pipe(
          tap(() => (this.completed = true)),
          catchError(() => {
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
