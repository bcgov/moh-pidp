import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';

import { EMPTY, Observable, tap } from 'rxjs';

import { AbstractFormPage } from '@app/core/classes/abstract-form-page.class';
import { FormUtilsService } from '@app/core/services/form-utils.service';
import { PartyService } from '@app/core/services/party.service';
import { Lookup } from '@app/modules/lookup/lookup.model';
import { LookupService } from '@app/modules/lookup/lookup.service';

import { CollegeLicenceInformationFormState } from './college-licence-information-form-state';
import { CollegeLicenceInformationResource } from './college-licence-information-resource.service';
import { CollegeLicenceInformationModel } from './college-licence-information.model';

@Component({
  selector: 'app-college-licence-information',
  templateUrl: './college-licence-information.component.html',
  styleUrls: ['./college-licence-information.component.scss'],
})
export class CollegeLicenceInformationComponent
  extends AbstractFormPage<CollegeLicenceInformationFormState>
  implements OnInit
{
  public title: string;
  public formState: CollegeLicenceInformationFormState;
  public colleges: Lookup[];
  public inGoodStanding: boolean;

  public constructor(
    protected dialog: MatDialog,
    protected formUtilsService: FormUtilsService,
    private route: ActivatedRoute,
    private router: Router,
    private partyService: PartyService,
    private resource: CollegeLicenceInformationResource,
    lookupService: LookupService,
    fb: FormBuilder
  ) {
    super(dialog, formUtilsService);

    this.title = this.route.snapshot.data.title;
    this.formState = new CollegeLicenceInformationFormState(fb);
    this.colleges = lookupService.colleges;
    this.inGoodStanding = false;
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
      .getCollegeLicenceInformation(partyId)
      .pipe(
        tap((model: CollegeLicenceInformationModel | null) =>
          this.formState.patchValue(model)
        )
      )
      .subscribe();
  }

  protected performSubmission(): Observable<void> {
    const partyId = 1; // +this.route.snapshot.params.pid;

    return this.formState.json
      ? this.resource.updateCollegeLicenceInformation(
          partyId,
          this.formState.json
        )
      : EMPTY;
  }

  protected afterSubmitIsSuccessful(): void {
    this.router.navigate(this.route.snapshot.data.route.root);
  }
}
