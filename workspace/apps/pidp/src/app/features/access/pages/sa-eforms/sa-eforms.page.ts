import { Component, Inject, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { Subscription, catchError, noop, of, tap } from 'rxjs';

import { APP_CONFIG, AppConfig } from '@app/app.config';
import { PartyService } from '@app/core/party/party.service';
import { DocumentService } from '@app/core/services/document.service';
import { LoggerService } from '@app/core/services/logger.service';
import { StatusCode } from '@app/features/portal/enums/status-code.enum';

import { SaEformsResource } from './sa-eforms-resource.service';
import { saEformsUrl } from './sa-eforms.constants';

@Component({
  selector: 'app-sa-eforms',
  templateUrl: './sa-eforms.page.html',
  styleUrls: ['./sa-eforms.page.scss'],
})
export class SaEformsPage implements OnInit {
  public busy?: Subscription;
  public title: string;
  public saEformsUrl: string;
  public collectionNotice: string;
  public completed: boolean | null;
  public accessRequestFailed: boolean;
  public specialAuthoritySupportEmail: string;

  public constructor(
    @Inject(APP_CONFIG) private config: AppConfig,
    private route: ActivatedRoute,
    private router: Router,
    private partyService: PartyService,
    private resource: SaEformsResource,
    private logger: LoggerService,
    documentService: DocumentService
  ) {
    const routeData = this.route.snapshot.data;
    this.title = routeData.title;
    this.saEformsUrl = saEformsUrl;
    this.collectionNotice = documentService.getSAeFormsCollectionNotice();
    this.completed = routeData.saEformsStatusCode === StatusCode.COMPLETED;
    this.accessRequestFailed = false;
    this.specialAuthoritySupportEmail =
      this.config.emails.specialAuthoritySupport;
  }

  public onBack(): void {
    this.navigateToRoot();
  }

  public onRequestAccess(): void {
    this.busy = this.resource
      .requestAccess(this.partyService.partyId)
      .pipe(
        tap(() => (this.completed = true)),
        catchError(() => {
          this.accessRequestFailed = true;
          return of(noop());
        })
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

  private navigateToRoot(): void {
    this.router.navigate([this.route.snapshot.data.routes.root]);
  }
}
