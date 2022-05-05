import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';

import { EMPTY, Observable } from 'rxjs';

import { AbstractFormPage } from '@app/core/classes/abstract-form-page.class';
import { PartyService } from '@app/core/party/party.service';
import { FormUtilsService } from '@app/core/services/form-utils.service';
import { LoggerService } from '@app/core/services/logger.service';
import { StatusCode } from '@app/features/portal/enums/status-code.enum';

import {
  healthNetBcHelpDeskEmail,
  healthNetBcHelpDeskPhone,
} from './hcim-enrolment-constants';
import { HcimEnrolmentFormState } from './hcim-enrolment-form-state';
import {
  HcimEnrolmentResource,
  HcimEnrolmentResponse,
  HcimEnrolmentStatusCode,
} from './hcim-enrolment-resource.service';
import { HcimEnrolment } from './hcim-enrolment.model';

@Component({
  selector: 'app-hcim-enrolment',
  templateUrl: './hcim-enrolment.page.html',
  styleUrls: ['./hcim-enrolment.page.scss'],
})
export class HcimEnrolmentPage
  extends AbstractFormPage<HcimEnrolmentFormState>
  implements OnInit
{
  public title: string;
  public formState: HcimEnrolmentFormState;
  public completed: boolean | null;
  public controls: { name: keyof HcimEnrolment; question: string }[];
  public accessRequestStatusCode?: HcimEnrolmentStatusCode;
  public formInvalid?: boolean;
  public readonly healthNetBcHelpDeskEmail: string;
  public readonly healthNetBcHelpDeskPhone: string;

  public HcimEnrolmentStatusCode = HcimEnrolmentStatusCode;

  public constructor(
    protected dialog: MatDialog,
    protected formUtilsService: FormUtilsService,
    private route: ActivatedRoute,
    private router: Router,
    private partyService: PartyService,
    private resource: HcimEnrolmentResource,
    private logger: LoggerService,
    fb: FormBuilder
  ) {
    super(dialog, formUtilsService);

    const routeData = this.route.snapshot.data;
    this.title = routeData.title;
    this.formState = new HcimEnrolmentFormState(fb);
    this.completed = routeData.hcimEnrolmentStatusCode === StatusCode.COMPLETED;
    this.controls = [
      {
        name: 'managesTasks',
        question: 'Do you manage tasks?',
      },
      {
        name: 'modifiesPhns',
        question: 'Do you need to create or update new PHNs?',
      },
      {
        name: 'recordsNewborns',
        question: 'Do you need to record newborns?',
      },
      {
        name: 'searchesIdentifiers',
        question: 'Do you need to search by source identifiers?',
      },
    ];
    this.healthNetBcHelpDeskEmail = healthNetBcHelpDeskEmail;
    this.healthNetBcHelpDeskPhone = healthNetBcHelpDeskPhone;
  }

  public onBack(): void {
    this.navigateToRoot();
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

  protected performSubmission(): Observable<HcimEnrolmentResponse> {
    const partyId = this.partyService.partyId;

    return partyId && this.formState.json
      ? this.resource.requestAccess(partyId, this.formState.json)
      : EMPTY;
  }

  protected onSubmitFormIsValid(): void {
    this.formInvalid = false;
  }

  protected onSubmitFormIsInvalid(): void {
    this.formInvalid = true;
  }

  protected afterSubmitIsSuccessful(
    accessResponse: HcimEnrolmentResponse
  ): void {
    const statusCode = accessResponse.statusCode;
    this.completed = statusCode === HcimEnrolmentStatusCode.ACCESS_GRANTED;
    this.accessRequestStatusCode = statusCode;
  }

  private navigateToRoot(): void {
    this.router.navigate([this.route.snapshot.data.routes.root]);
  }
}
