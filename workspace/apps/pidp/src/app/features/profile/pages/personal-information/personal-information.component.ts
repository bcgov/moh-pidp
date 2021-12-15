import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';

import { Address, BcscUser } from '@bcgov/shared/data-access';
import { FormControlValidators, ToggleContentChange } from '@bcgov/shared/ui';

@Component({
  selector: 'app-personal-information',
  templateUrl: './personal-information.component.html',
  styleUrls: ['./personal-information.component.scss'],
})
export class PersonalInformationComponent implements OnInit {
  public title: string;
  public bcscUser: BcscUser;

  public form!: FormGroup;

  public constructor(private fb: FormBuilder, private route: ActivatedRoute) {
    this.title = this.route.snapshot.data.title;
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
    this.form = this.fb.group({
      preferredFirstName: [null, []],
      preferredMiddleName: [null, []],
      preferredLastName: [null, []],
      mailingAddress: this.fb.group({
        countryCode: [{ value: null, disabled: false }, []],
        provinceCode: [{ value: null, disabled: false }, []],
        street: [{ value: null, disabled: false }, []],
        city: [{ value: null, disabled: false }, []],
        postal: [{ value: null, disabled: false }, []],
      }),
      phone: [null, [Validators.required, FormControlValidators.phone]],
      email: [null, [Validators.required, FormControlValidators.email]],
    });
  }
}
