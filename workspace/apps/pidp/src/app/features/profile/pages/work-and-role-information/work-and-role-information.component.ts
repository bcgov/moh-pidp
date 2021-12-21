import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';

import { EMPTY, Observable, tap } from 'rxjs';

import { LookupService } from '@app/modules/lookup/lookup.service';

import { AbstractFormPage } from '@core/classes/abstract-form-page.class';
import { FormUtilsService } from '@core/services/form-utils.service';
import { PartyService } from '@core/services/party.service';

import { WorkAndRoleInformationFormState } from './work-and-role-information-form-state';
import { WorkAndRoleInformationResource } from './work-and-role-information-resource.service';
import { WorkAndRoleInformationModel } from './work-and-role-information.model';

@Component({
  selector: 'app-work-and-role-information',
  templateUrl: './work-and-role-information.component.html',
  styleUrls: ['./work-and-role-information.component.scss'],
})
export class WorkAndRoleInformationComponent
  extends AbstractFormPage<WorkAndRoleInformationFormState>
  implements OnInit
{
  public title: string;
  public formState: WorkAndRoleInformationFormState;

  public constructor(
    protected dialog: MatDialog,
    protected formUtilsService: FormUtilsService,
    private route: ActivatedRoute,
    private router: Router,
    private partyService: PartyService,
    private resource: WorkAndRoleInformationResource,
    lookupService: LookupService,
    fb: FormBuilder
  ) {
    super(dialog, formUtilsService);

    this.title = this.route.snapshot.data.title;
    this.formState = new WorkAndRoleInformationFormState(fb);
  }

  public onBack(): void {
    this.router.navigate(this.route.snapshot.data.route.root);
  }

  public ngOnInit(): void {
    const partyId = 1; // +this.route.snapshot.params.pid;
    if (!partyId) {
      throw new Error('No party ID was provided');
    }

    this.resource
      .getWorkAndRoleInformation(partyId)
      .pipe(
        tap((model: WorkAndRoleInformationModel | null) =>
          this.formState.patchValue(model)
        )
      )
      .subscribe();
  }

  protected performSubmission(): Observable<void> {
    const partyId = 1; // +this.route.snapshot.params.pid;

    return this.formState.json
      ? this.resource.updateWorkAndRoleInformation(partyId, this.formState.json)
      : EMPTY;
  }

  protected afterSubmitIsSuccessful(): void {
    this.router.navigate(this.route.snapshot.data.route.root);
  }
}
