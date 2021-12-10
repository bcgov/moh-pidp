import { Component, Input, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';

// TODO copy address-form from PRIME
@Component({
  selector: 'ui-address-form',
  templateUrl: './address-form.component.html',
  styleUrls: ['./address-form.component.scss'],
})
export class AddressFormComponent implements OnInit {
  @Input() public form!: FormGroup;

  public constructor() {}

  public ngOnInit(): void {}
}
