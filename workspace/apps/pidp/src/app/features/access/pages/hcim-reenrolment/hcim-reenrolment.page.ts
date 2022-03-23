import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';

import { EMPTY, Observable } from 'rxjs';

import { AbstractFormPage } from '@app/core/classes/abstract-form-page.class';
import { PartyService } from '@app/core/party/party.service';
import { FormUtilsService } from '@app/core/services/form-utils.service';
import { LoggerService } from '@app/core/services/logger.service';

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

    this.title = this.route.snapshot.data.title;
    this.formState = new HcimReenrolmentFormState(fb);
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
