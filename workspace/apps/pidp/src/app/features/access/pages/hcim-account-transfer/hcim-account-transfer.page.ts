import { NgIf } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { ActivatedRoute, Router } from '@angular/router';

import { EMPTY, Observable } from 'rxjs';

import {
  AlertActionsDirective,
  AlertComponent,
  AlertContentDirective,
  AnchorDirective,
  PageComponent,
  PageFooterActionDirective,
  PageFooterComponent,
  PageHeaderComponent,
  PageSectionComponent,
  PageSectionSubheaderComponent,
  PageSectionSubheaderDescDirective,
  PageSectionSubheaderHintDirective,
  PageSubheaderComponent,
} from '@bcgov/shared/ui';

import {
  AbstractFormDependenciesService,
  AbstractFormPage,
} from '@app/core/classes/abstract-form-page.class';
import { PartyService } from '@app/core/party/party.service';
import { LoggerService } from '@app/core/services/logger.service';
import { StatusCode } from '@app/features/portal/enums/status-code.enum';

import {
  hcimWebUrl,
  healthNetBcAdminEmail,
  healthNetBcAdminPhone,
  healthNetBcHelpDeskEmail,
  healthNetBcHelpDeskPhone,
  healthNetBcPasswordResetUrl,
} from './hcim-account-transfer-constants';
import { HcimAccountTransferFormState } from './hcim-account-transfer-form-state';
import {
  HcimAccountTransferResource,
  HcimAccountTransferResponse,
  HcimAccountTransferStatusCode,
} from './hcim-account-transfer-resource.service';

@Component({
  selector: 'app-hcim-account-transfer',
  templateUrl: './hcim-account-transfer.page.html',
  styleUrls: ['./hcim-account-transfer.page.scss'],
  viewProviders: [HcimAccountTransferResource],
  standalone: true,
  imports: [
    AlertActionsDirective,
    AlertComponent,
    AlertContentDirective,
    AnchorDirective,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    NgIf,
    PageComponent,
    PageFooterActionDirective,
    PageFooterComponent,
    PageHeaderComponent,
    PageSectionComponent,
    PageSectionSubheaderComponent,
    PageSectionSubheaderDescDirective,
    PageSectionSubheaderHintDirective,
    PageSubheaderComponent,
    ReactiveFormsModule,
  ],
})
export class HcimAccountTransferPage
  extends AbstractFormPage<HcimAccountTransferFormState>
  implements OnInit
{
  public title: string;
  public formState: HcimAccountTransferFormState;
  public completed: boolean | null;
  public accessRequestStatusCode?: HcimAccountTransferStatusCode;
  public loginAttempts: number;
  public readonly maxLoginAttempts: number;
  public readonly hcimWebUrl: string;
  public readonly healthNetBcPasswordResetUrl: string;
  public readonly healthNetBcHelpDeskEmail: string;
  public readonly healthNetBcHelpDeskPhone: string;
  public readonly healthRegistriesAdminEmail: string;
  public readonly healthRegistriesAdminPhone: string;

  public HcimAccountTransferStatusCode = HcimAccountTransferStatusCode;

  // ui-page is handling this.
  public showOverlayOnSubmit = false;

  public constructor(
    dependenciesService: AbstractFormDependenciesService,
    private route: ActivatedRoute,
    private router: Router,
    private partyService: PartyService,
    private resource: HcimAccountTransferResource,
    private logger: LoggerService,
    fb: FormBuilder,
  ) {
    super(dependenciesService);

    const routeData = this.route.snapshot.data;
    this.title = routeData.title;
    this.formState = new HcimAccountTransferFormState(fb);
    this.completed =
      routeData.hcimAccountTransferStatusCode === StatusCode.COMPLETED;
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

  protected performSubmission(): Observable<HcimAccountTransferResponse> {
    const partyId = this.partyService.partyId;

    return partyId && this.formState.json
      ? this.resource.requestAccess(partyId, this.formState.json)
      : EMPTY;
  }

  protected afterSubmitIsSuccessful(
    accessResponse: HcimAccountTransferResponse,
  ): void {
    const statusCode = accessResponse.statusCode;
    const remainingAttempts =
      accessResponse.remainingAttempts ?? this.maxLoginAttempts;

    this.completed =
      statusCode === HcimAccountTransferStatusCode.ACCESS_GRANTED;
    this.accessRequestStatusCode = statusCode;
    this.loginAttempts = this.maxLoginAttempts - remainingAttempts;
  }

  private navigateToRoot(): void {
    this.router.navigate([this.route.snapshot.data.routes.root]);
  }
}
