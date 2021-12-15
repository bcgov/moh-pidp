import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';

import { Address, BcscUser } from '@bcgov/shared/data-access';
import { AbstractFormPage, ToggleContentChange } from '@bcgov/shared/ui';
import { FormUtilsService } from '@core/services/form-utils.service';

import { ProfileInformationFormState } from './personal-information-form-state';

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

  public formState: ProfileInformationFormState;
  public form!: FormGroup;

  public constructor(
    private formUtilsService: FormUtilsService,
    private route: ActivatedRoute,
    fb: FormBuilder
  ) {
    super();

    this.title = this.route.snapshot.data.title;
    this.formState = new ProfileInformationFormState(fb, formUtilsService);

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

  public get mailingAddress(): FormGroup {
    return this.form.get('mailingAddress') as FormGroup;
  }

  public onSubmit(): void {}

  public onPreferredNameToggle({ checked }: ToggleContentChange): void {}

  public onAddressToggle({ checked }: ToggleContentChange): void {}

  public ngOnInit(): void {
    this.initForm();
  }

  private initForm(): void {
    this.form = this.formState.form;
  }

  private patchForm(): void {}
}
