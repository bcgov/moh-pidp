import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute } from '@angular/router';

import { EMPTY, Observable, tap } from 'rxjs';

import { Address, BcscUser } from '@bcgov/shared/data-access';
import { ToggleContentChange } from '@bcgov/shared/ui';
import { AbstractFormPage } from '@core/classes/abstract-form-page.class';
import { DemoService } from '@core/services/demo.service';
import { FormUtilsService } from '@core/services/form-utils.service';

import { PersonalInformationFormState } from './personal-information-form-state';
import { PersonalInformationResource } from './personal-information-resource.service';
import { PersonalInformationModel } from './personal-information.model';

@Component({
  selector: 'app-personal-information',
  templateUrl: './personal-information.component.html',
  styleUrls: ['./personal-information.component.scss'],
})
export class PersonalInformationComponent
  extends AbstractFormPage
  implements OnInit
{
  public title: string;
  public bcscUser: BcscUser;

  public formState: PersonalInformationFormState;
  public personalInformation$!: Observable<PersonalInformationModel | null>;

  public constructor(
    protected dialog: MatDialog,
    protected formUtilsService: FormUtilsService,
    private route: ActivatedRoute,
    private resource: PersonalInformationResource,
    private demoService: DemoService,
    fb: FormBuilder
  ) {
    super(dialog, formUtilsService);

    this.title = this.route.snapshot.data.title;
    this.formState = new PersonalInformationFormState(fb);

    this.bcscUser = {
      hpdid: '00000000-0000-0000-0000-000000000000',
      firstName: 'Jamie',
      lastName: 'Dormaar',
      dateOfBirth: '1983-05-17T00:00:00',
      verifiedAddress: new Address(
        'CA',
        'BC',
        '140 Beach Dr.',
        'Victoria',
        'V8S 2L5'
      ),
    };
  }

  public onSubmit(): void {
    this.demoService.state.profileIdentitySections =
      this.demoService.state.profileIdentitySections.map((section) => {
        if (section.type === 'personal-information') {
          return {
            ...section,
            statusType: 'success',
            status: 'completed',
          };
        }
        if (section.type === 'college-licence-information') {
          return {
            ...section,
            disabled: false,
          };
        }
        return section;
      });
  }

  public onPreferredNameToggle({ checked }: ToggleContentChange): void {}

  public onAddressToggle({ checked }: ToggleContentChange): void {}

  public ngOnInit(): void {
    // const partyId = +this.route.snapshot.params.pid;
    // if (!partyId) {
    //   throw new Error('No party ID was provided');
    // }
    // this.personalInformation$ = this.resource
    //   .getProfileInformation(partyId)
    //   .pipe(
    //     tap((model: PersonalInformationModel | null) =>
    //       this.formState.patchValue(model)
    //     )
    //   );
  }

  protected performSubmission(): Observable<void> {
    const partyId = +this.route.snapshot.params.pid;

    return this.formState.json
      ? this.resource.updatePersonalInformation(partyId, this.formState.json)
      : EMPTY;
  }
}
