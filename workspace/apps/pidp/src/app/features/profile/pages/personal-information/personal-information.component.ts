import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';

import { EMPTY, Observable, tap } from 'rxjs';

import { BcscUser } from '@bcgov/shared/data-access';
import { ToggleContentChange } from '@bcgov/shared/ui';

import { AbstractFormPage } from '@core/classes/abstract-form-page.class';
import { FormUtilsService } from '@core/services/form-utils.service';
import { PartyService } from '@core/services/party.service';

import { PersonalInformationFormState } from './personal-information-form-state';
import { PersonalInformationResource } from './personal-information-resource.service';
import { PersonalInformationModel } from './personal-information.model';

@Component({
  selector: 'app-personal-information',
  templateUrl: './personal-information.component.html',
  styleUrls: ['./personal-information.component.scss'],
  viewProviders: [PersonalInformationResource],
})
export class PersonalInformationComponent
  extends AbstractFormPage<PersonalInformationFormState>
  implements OnInit
{
  public title: string;
  public formState: PersonalInformationFormState;
  public bcscUser: BcscUser;

  public constructor(
    protected dialog: MatDialog,
    protected formUtilsService: FormUtilsService,
    private route: ActivatedRoute,
    private router: Router,
    private partyService: PartyService,
    private resource: PersonalInformationResource,
    fb: FormBuilder
  ) {
    super(dialog, formUtilsService);

    this.title = this.route.snapshot.data.title;
    this.formState = new PersonalInformationFormState(fb);
    this.bcscUser = this.partyService.user;
  }

  public onPreferredNameToggle({ checked }: ToggleContentChange): void {
    this.formUtilsService.setOrResetValidators(
      checked,
      this.formState.preferredFirstName
    );
    this.formUtilsService.setOrResetValidators(
      checked,
      this.formState.preferredLastName
    );
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
        tap((model: PersonalInformationModel | null) =>
          this.formState.patchValue(model)
        )
      )
      .subscribe();
  }

  protected performSubmission(): Observable<void> {
    const partyId = 1; // +this.route.snapshot.params.pid;

    return this.formState.json
      ? this.resource.update(partyId, this.formState.json)
      : EMPTY;
  }

  protected afterSubmitIsSuccessful(): void {
    this.router.navigate([this.route.snapshot.data.routes.root]);
  }
}
