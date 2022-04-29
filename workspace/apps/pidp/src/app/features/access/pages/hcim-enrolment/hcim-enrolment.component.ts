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

import { HcimEnrolmentFormState } from './hcim-enrolment-form-state';
import {
  HcimEnrolmentResource,
  HcimEnrolmentResponse,
} from './hcim-enrolment-resource.service';

@Component({
  selector: 'app-hcim-enrolment',
  templateUrl: './hcim-enrolment.component.html',
  styleUrls: ['./hcim-enrolment.component.scss'],
})
export class HcimEnrolmentComponent
  extends AbstractFormPage<HcimEnrolmentFormState>
  implements OnInit
{
  public title: string;
  public formState: HcimEnrolmentFormState;
  public completed: boolean | null;

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

  protected afterSubmitIsSuccessful(
    accessResponse: HcimEnrolmentResponse
  ): void {}

  private navigateToRoot(): void {
    this.router.navigate([this.route.snapshot.data.routes.root]);
  }
}
