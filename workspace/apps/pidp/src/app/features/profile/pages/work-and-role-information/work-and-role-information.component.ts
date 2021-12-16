import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';

import { DemoService } from '@core/services/demo.service';

@Component({
  selector: 'app-work-and-role-information',
  templateUrl: './work-and-role-information.component.html',
  styleUrls: ['./work-and-role-information.component.scss'],
})
export class WorkAndRoleInformationComponent implements OnInit {
  public title: string;
  public form!: FormGroup;

  public constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private demoService: DemoService
  ) {
    this.title = this.route.snapshot.data.title;
  }

  public get physicalAddress(): FormGroup {
    return this.form.get('physicalAddress') as FormGroup;
  }

  public onSubmit(): void {
    this.demoService.state.profileIdentitySections =
      this.demoService.state.profileIdentitySections.map((section) => {
        if (section.type === 'work-and-role-information') {
          return {
            ...section,
            statusType: 'success',
            status: 'completed',
          };
        }
        if (section.type === 'terms-of-access-agreement') {
          return {
            ...section,
            disabled: false,
          };
        }
        return section;
      });
  }

  public ngOnInit(): void {
    this.form = this.fb.group({
      physicalAddress: this.fb.group({
        countryCode: [{ value: null, disabled: false }, []],
        provinceCode: [{ value: null, disabled: false }, []],
        street: [{ value: null, disabled: false }, []],
        city: [{ value: null, disabled: false }, []],
        postal: [{ value: null, disabled: false }, []],
      }),
    });
  }
}
