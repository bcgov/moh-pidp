import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-work-and-role-information',
  templateUrl: './work-and-role-information.component.html',
  styleUrls: ['./work-and-role-information.component.scss'],
})
export class WorkAndRoleInformationComponent implements OnInit {
  public title: string;
  public form!: FormGroup;

  public constructor(private fb: FormBuilder, private route: ActivatedRoute) {
    this.title = this.route.snapshot.data.title;
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
