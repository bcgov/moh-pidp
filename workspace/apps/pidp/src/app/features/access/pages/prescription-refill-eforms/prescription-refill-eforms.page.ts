import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { catchError, noop, of, tap } from 'rxjs';

import { PartyService } from '@app/core/party/party.service';
import { DocumentService } from '@app/core/services/document.service';
import { LoggerService } from '@app/core/services/logger.service';
import { StatusCode } from '@app/features/portal/enums/status-code.enum';

import { PrescriptionRefillEformsResource } from './prescription-refill-eforms-resource.service';
import {
  prescriptionRefillRequestEformsSupportEmail,
  prescriptionRefillRequestEformsUrl,
} from './prescription-refill-eforms.constants';

@Component({
  selector: 'app-prescription-refill-eforms',
  templateUrl: './prescription-refill-eforms.page.html',
  styleUrls: ['./prescription-refill-eforms.page.scss'],
})
export class PrescriptionRefillEformsPage implements OnInit {
  public title: string;
  public collectionNotice: string;
  public completed: boolean | null;
  public accessRequestFailed: boolean;
  public prescriptionRefillEformsUrl: string;
  public prescriptionRefillEformsSupportEmail: string;
  public enrolmentError: boolean;

  public constructor(
    private route: ActivatedRoute,
    private router: Router,
    private partyService: PartyService,
    private resource: PrescriptionRefillEformsResource,
    private logger: LoggerService,
    private documentService: DocumentService
  ) {
    const routeData = this.route.snapshot.data;
    this.title = routeData.title;
    this.collectionNotice =
      documentService.getPrescriptionRefilleFormsCollectionNotice();
    this.completed =
      routeData.prescriptionRefillEformsStatusCode === StatusCode.COMPLETED;
    this.accessRequestFailed = false;
    this.enrolmentError = false;
    this.prescriptionRefillEformsUrl = prescriptionRefillRequestEformsUrl;
    this.prescriptionRefillEformsSupportEmail =
      prescriptionRefillRequestEformsSupportEmail;
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
