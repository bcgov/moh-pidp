import { NgIf } from '@angular/common';
import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { ActivatedRoute, Router } from '@angular/router';

import { catchError, noop, of, tap } from 'rxjs';

import {
  AlertComponent,
  AlertContentDirective,
  AnchorDirective,
  InjectViewportCssClassDirective,
  PageComponent,
  PageFooterActionDirective,
  PageFooterComponent,
  PageHeaderComponent,
  PageSectionComponent,
  PageSectionSubheaderComponent,
  PageSectionSubheaderDescDirective,
  SafePipe,
  TextButtonDirective,
} from '@bcgov/shared/ui';

import { PartyService } from '@app/core/party/party.service';
import { DocumentService } from '@app/core/services/document.service';
import { LoggerService } from '@app/core/services/logger.service';
import { StatusCode } from '@app/features/portal/enums/status-code.enum';

import { EnrolmentErrorComponent } from '../../components/enrolment-error/enrolment-error.component';
import { SaEformsResource } from './sa-eforms-resource.service';
import {
  specialAuthorityEformsSupportEmail,
  specialAuthorityEformsUrl,
} from './sa-eforms.constants';
import { FaIconComponent } from '@fortawesome/angular-fontawesome';
import { faAngleRight } from '@fortawesome/free-solid-svg-icons';
import { AccessRoutes } from '../../access.routes';

@Component({
  selector: 'app-sa-eforms',
  templateUrl: './sa-eforms.page.html',
  styleUrls: ['./sa-eforms.page.scss'],
  standalone: true,
  imports: [
    AlertComponent,
    AlertContentDirective,
    AnchorDirective,
    EnrolmentErrorComponent,
    FaIconComponent,
    InjectViewportCssClassDirective,
    MatButtonModule,
    NgIf,
    PageComponent,
    PageFooterActionDirective,
    PageFooterComponent,
    PageHeaderComponent,
    PageSectionComponent,
    PageSectionSubheaderComponent,
    PageSectionSubheaderDescDirective,
    SafePipe,
    TextButtonDirective,
  ],
})
export class SaEformsPage implements OnInit {
  public title: string;
  public collectionNotice: string;
  public completed: boolean | null;
  public accessRequestFailed: boolean;
  public specialAuthorityEformsUrl: string;
  public specialAuthoritySupportEmail: string;
  public enrolmentError: boolean;
  public faAngleRight = faAngleRight;
  public AccessRoutes = AccessRoutes;

  public constructor(
    private route: ActivatedRoute,
    private router: Router,
    private partyService: PartyService,
    private resource: SaEformsResource,
    private logger: LoggerService,
    documentService: DocumentService,
  ) {
    const routeData = this.route.snapshot.data;
    this.title = routeData.title;
    this.collectionNotice = documentService.getSAeFormsCollectionNotice();
    this.completed = routeData.saEformsStatusCode === StatusCode.COMPLETED;
    this.accessRequestFailed = false;
    this.specialAuthorityEformsUrl = specialAuthorityEformsUrl;
    this.specialAuthoritySupportEmail = specialAuthorityEformsSupportEmail;
    this.enrolmentError = false;
  }

  public onBack(): void {
    this.navigateToRoot();
  }

  public onRequestAccess(): void {
    this.resource
      .requestAccess(this.partyService.partyId)
      .pipe(
        tap(() => {
          this.completed = true;
          this.enrolmentError = false;
        }),
        catchError((error: HttpErrorResponse) => {
          if (error.status === HttpStatusCode.BadRequest) {
            this.completed = false;
            this.enrolmentError = true;
            return of(noop());
          }
          this.accessRequestFailed = true;
          return of(noop());
        }),
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
  public navigateTo(path: string): void {
    this.router.navigateByUrl(path);
  }

  private navigateToRoot(): void {
    this.router.navigate([this.route.snapshot.data.routes.root]);
  }
}
