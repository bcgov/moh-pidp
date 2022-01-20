import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';

import { EMPTY, Observable, tap } from 'rxjs';

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
  viewProviders: [WorkAndRoleInformationResource],
})
export class WorkAndRoleInformationComponent
  extends AbstractFormPage<WorkAndRoleInformationFormState>
  implements OnInit
{
  public title: string;
  public formState: WorkAndRoleInformationFormState;
  public hasPhysicalAddress: boolean;

  public constructor(
    protected dialog: MatDialog,
    protected formUtilsService: FormUtilsService,
    private route: ActivatedRoute,
    private router: Router,
    // TODO switch to RxJS state management using Elf
    private partyService: PartyService,
    private resource: WorkAndRoleInformationResource,
    fb: FormBuilder
  ) {
    super(dialog, formUtilsService);

    this.title = this.route.snapshot.data.title;
    this.formState = new WorkAndRoleInformationFormState(fb, formUtilsService);
    this.hasPhysicalAddress = false;
  }

  public onBack(): void {
    this.router.navigate([this.route.snapshot.data.routes.root]);
  }

  public ngOnInit(): void {
    // TODO pull from state management or URI param
    const partyId = 1; // +this.route.snapshot.params.pid;
    if (!partyId) {
      throw new Error('No party ID was provided');
    }

    this.resource
      .get(partyId)
      .pipe(
        tap((model: WorkAndRoleInformationModel | null) =>
          this.formState.patchValue(model)
        )
      )
      .subscribe(
        (model: WorkAndRoleInformationModel | null) =>
          (this.hasPhysicalAddress = !!model?.physicalAddress)
      );
  }

  protected performSubmission(): Observable<void> {
    const partyId = 1; // +this.route.snapshot.params.pid;

    return this.formState.json
      ? this.resource.update(partyId, this.formState.json)
      : EMPTY;
  }

  protected afterSubmitIsSuccessful(): void {
    this.partyService.updateState('work-and-role-information');

    this.router.navigate([this.route.snapshot.data.routes.root]);
  }
}
