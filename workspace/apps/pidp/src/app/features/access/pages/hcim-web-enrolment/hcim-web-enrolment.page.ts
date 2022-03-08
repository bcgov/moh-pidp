import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';

import { EMPTY, Observable } from 'rxjs';

import { AbstractFormPage } from '@app/core/classes/abstract-form-page.class';
import { FormUtilsService } from '@app/core/services/form-utils.service';
import { LoggerService } from '@app/core/services/logger.service';
import { PartyService } from '@app/core/services/party.service';

import { HcimWebEnrolmentFormState } from './hcim-web-enrolment-form-state';
import { HcimWebEnrolmentResource } from './hcim-web-enrolment-resource.service';

@Component({
  selector: 'app-hcim-web-enrolment',
  templateUrl: './hcim-web-enrolment.page.html',
  styleUrls: ['./hcim-web-enrolment.page.scss'],
  viewProviders: [HcimWebEnrolmentResource],
})
export class HcimWebEnrolmentPage
  extends AbstractFormPage<HcimWebEnrolmentFormState>
  implements OnInit
{
  public title: string;
  public formState: HcimWebEnrolmentFormState;

  public constructor(
    protected dialog: MatDialog,
    // TODO replace dialog with dialogService
    // protected dialogService: DialogService,
    protected formUtilsService: FormUtilsService,
    private route: ActivatedRoute,
    private router: Router,
    private partyService: PartyService,
    private resource: HcimWebEnrolmentResource,
    private logger: LoggerService,
    fb: FormBuilder
  ) {
    super(dialog, formUtilsService);

    this.title = this.route.snapshot.data.title;
    this.formState = new HcimWebEnrolmentFormState(fb);
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

    // TODO perform request to determine whether completed
    // and show the appropriate markup based on response
    // this.resource.get(partyId).subscribe();
  }

  protected performSubmission(): Observable<void> {
    const partyId = this.partyService.partyId;

    return partyId && this.formState.json
      ? this.resource.update(partyId, this.formState.json)
      : EMPTY;
  }

  protected afterSubmitIsSuccessful(): void {
    this.navigateToRoot();
  }

  private navigateToRoot(): void {
    this.router.navigate([this.route.snapshot.data.routes.root]);
  }
}
