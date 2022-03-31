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
  hcimWebUrl,
  healthNetBcHelpDeskEmail,
  healthNetBcHelpDeskPhone,
  healthNetBcPasswordResetUrl,
} from './hcim-constants';
import { HcimReenrolmentFormState } from './hcim-reenrolment-form-state';
import { HcimReenrolmentResource } from './hcim-reenrolment-resource.service';

@Component({
  selector: 'app-hcim-reenrolment',
  templateUrl: './hcim-reenrolment.page.html',
  styleUrls: ['./hcim-reenrolment.page.scss'],
  viewProviders: [HcimReenrolmentResource],
})
export class HcimReenrolmentPage
  extends AbstractFormPage<HcimReenrolmentFormState>
  implements OnInit
{
  public title: string;
  public formState: HcimReenrolmentFormState;
  public completed: boolean | null;
  public accessRequestFailed: boolean;
  public accountLocked: boolean;
  public loginAttempts: number;
  public readonly maxLoginAttempts: number;
  public readonly hcimWebUrl: string;
  public readonly healthNetBcPasswordResetUrl: string;
  public readonly healthNetBcHelpDeskEmail: string;
  public readonly healthNetBcHelpDeskPhone: string;

  public constructor(
    protected dialog: MatDialog,
    protected formUtilsService: FormUtilsService,
    private route: ActivatedRoute,
    private router: Router,
    private partyService: PartyService,
    private resource: HcimReenrolmentResource,
    private logger: LoggerService,
    fb: FormBuilder
  ) {
    super(dialog, formUtilsService);

    const routeData = this.route.snapshot.data;
    this.title = routeData.title;
    this.formState = new HcimReenrolmentFormState(fb);
    this.completed =
      routeData.hcimReenrolmentStatusCode === StatusCode.COMPLETED;
    this.accessRequestFailed = false;
    this.accountLocked = false;
    this.loginAttempts = 0;
    this.maxLoginAttempts = 3;
    this.hcimWebUrl = hcimWebUrl;
    this.healthNetBcPasswordResetUrl = healthNetBcPasswordResetUrl;
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

  protected performSubmission(): Observable<void> {
    const partyId = this.partyService.partyId;

    // TODO handle account lock when headers are provided

    return partyId && this.formState.json
      ? this.resource.requestAccess(partyId, this.formState.json)
      : EMPTY;
  }

  protected onSubmitFormIsValid(): void {
    this.loginAttempts += 1;
  }

  protected afterSubmitIsSuccessful(): void {
    this.navigateToRoot();
  }

  private navigateToRoot(): void {
    this.router.navigate([this.route.snapshot.data.routes.root]);
  }
}
