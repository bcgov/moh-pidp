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
  healthNetBcAdminEmail,
  healthNetBcAdminPhone,
  healthNetBcHelpDeskEmail,
  healthNetBcHelpDeskPhone,
  healthNetBcPasswordResetUrl,
} from './hcimweb-account-transfer-constants';
import { HcimwebAccountTransferFormState } from './hcimweb-account-transfer-form-state';
import {
  HcimAccessRequestResponse,
  HcimAccessRequestStatusCode,
  HcimwebAccountTransferResource,
} from './hcimweb-account-transfer-resource.service';

@Component({
  selector: 'app-hcimweb-account-transfer',
  templateUrl: './hcimweb-account-transfer.page.html',
  styleUrls: ['./hcimweb-account-transfer.page.scss'],
  viewProviders: [HcimwebAccountTransferResource],
})
export class HcimwebAccountTransferPage
  extends AbstractFormPage<HcimwebAccountTransferFormState>
  implements OnInit
{
  public title: string;
  public formState: HcimwebAccountTransferFormState;
  public completed: boolean | null;
  public accessRequestStatusCode?: HcimAccessRequestStatusCode;
  public loginAttempts: number;
  public readonly maxLoginAttempts: number;
  public readonly hcimWebUrl: string;
  public readonly healthNetBcPasswordResetUrl: string;
  public readonly healthNetBcHelpDeskEmail: string;
  public readonly healthNetBcHelpDeskPhone: string;
  public readonly healthRegistriesAdminEmail: string;
  public readonly healthRegistriesAdminPhone: string;

  public HcimAccessRequestStatusCode = HcimAccessRequestStatusCode;

  public constructor(
    protected dialog: MatDialog,
    protected formUtilsService: FormUtilsService,
    private route: ActivatedRoute,
    private router: Router,
    private partyService: PartyService,
    private resource: HcimwebAccountTransferResource,
    private logger: LoggerService,
    fb: FormBuilder
  ) {
    super(dialog, formUtilsService);

    const routeData = this.route.snapshot.data;
    this.title = routeData.title;
    this.formState = new HcimwebAccountTransferFormState(fb);
    this.completed =
      routeData.hcimwebAccountTransferStatusCode === StatusCode.COMPLETED;
    this.loginAttempts = 0;
    this.maxLoginAttempts = 3;
    this.hcimWebUrl = hcimWebUrl;
    this.healthNetBcPasswordResetUrl = healthNetBcPasswordResetUrl;
    this.healthNetBcHelpDeskEmail = healthNetBcHelpDeskEmail;
    this.healthNetBcHelpDeskPhone = healthNetBcHelpDeskPhone;
    this.healthRegistriesAdminEmail = healthNetBcAdminEmail;
    this.healthRegistriesAdminPhone = healthNetBcAdminPhone;
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

  protected performSubmission(): Observable<HcimAccessRequestResponse> {
    const partyId = this.partyService.partyId;

    return partyId && this.formState.json
      ? this.resource.requestAccess(partyId, this.formState.json)
      : EMPTY;
  }

  protected afterSubmitIsSuccessful(
    accessResponse: HcimAccessRequestResponse
  ): void {
    const statusCode = accessResponse.statusCode;
    const remainingAttempts =
      accessResponse.remainingAttempts ?? this.maxLoginAttempts;

    this.completed = statusCode === HcimAccessRequestStatusCode.ACCESS_GRANTED;
    this.accessRequestStatusCode = statusCode;
    this.loginAttempts = this.maxLoginAttempts - remainingAttempts;
  }

  private navigateToRoot(): void {
    this.router.navigate([this.route.snapshot.data.routes.root]);
  }
}
