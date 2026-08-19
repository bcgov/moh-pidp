import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';
import { AfterViewInit, Component, OnInit, inject } from '@angular/core';
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
  PageSectionComponent,
  PageSectionSubheaderComponent,
  PageSectionSubheaderDescDirective,
  SafePipe,
} from '@bcgov/shared/ui';

import { PartyService } from '@app/core/party/party.service';
import { DocumentService } from '@app/core/services/document.service';
import { LoggerService } from '@app/core/services/logger.service';
import { SnowplowService } from '@app/core/services/snowplow.service';
import { StatusCode } from '@app/features/portal/enums/status-code.enum';
import { BreadcrumbComponent } from '@app/shared/components/breadcrumb/breadcrumb.component';

import { AccessRoutes } from '../../access.routes';
import { EnrolmentErrorComponent } from '../../components/enrolment-error/enrolment-error.component';
import { NpdpEformsResource } from './npdp-eforms-resource.service';
import {
  npdpEformsSupportEmail,
  npdpEformsUrl,
} from './npdp-eforms.constants';

@Component({
  selector: 'app-npdp-eforms',
  templateUrl: './npdp-eforms.page.html',
  styleUrls: ['./npdp-eforms.page.scss'],
  imports: [
    AlertComponent,
    AlertContentDirective,
    AnchorDirective,
    BreadcrumbComponent,
    EnrolmentErrorComponent,
    InjectViewportCssClassDirective,
    MatButtonModule,
    PageComponent,
    PageFooterActionDirective,
    PageFooterComponent,
    PageSectionComponent,
    PageSectionSubheaderComponent,
    PageSectionSubheaderDescDirective,
    SafePipe,
  ],
})
export class NpdpEformsPage implements OnInit, AfterViewInit {
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly partyService = inject(PartyService);
  private readonly resource = inject(NpdpEformsResource);
  private readonly logger = inject(LoggerService);
  private readonly snowplowService = inject(SnowplowService);

  public title: string;
  public collectionNotice: string;
  public completed: boolean | null;
  public accessRequestFailed: boolean;
  public npdpEformsUrl: string;
  public npdpEformsSupportEmail: string;
  public enrolmentError: boolean;
  public AccessRoutes = AccessRoutes;
  public breadcrumbsData: Array<{ title: string; path: string }> = [
    { title: 'Home', path: '' },
    {
      title: 'Access',
      path: AccessRoutes.routePath(AccessRoutes.ACCESS_REQUESTS),
    },
    { title: 'Exceptional Coverage: National PharmaCare and Mifepristone/Misoprostol', path: '' },
  ];

  public constructor() {
    const documentService = inject(DocumentService);

    const routeData = this.route.snapshot.data;
    this.title = routeData.title;
    this.collectionNotice =
      documentService.getNpdpEformsCollectionNotice();
    this.completed =
      routeData.npdpEformsStatusCode === StatusCode.COMPLETED;
    this.accessRequestFailed = false;
    this.npdpEformsUrl = npdpEformsUrl;
    this.npdpEformsSupportEmail = npdpEformsSupportEmail;
    this.enrolmentError = false;
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

  public ngAfterViewInit(): void {
    this.snowplowService.refreshLinkClickTracking();
  }

  private navigateToRoot(): void {
    this.router.navigate([this.route.snapshot.data.routes.root]);
  }
}
