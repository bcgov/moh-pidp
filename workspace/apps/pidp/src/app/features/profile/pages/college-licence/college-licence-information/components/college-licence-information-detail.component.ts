import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-college-licence-information-detail',
  template: `
    <span
      ><strong>{{ labelText }}</strong
      >&nbsp;<span>{{ valueText ?? 'Not Available' }}</span></span
    >
  `,
  styles: [''],
  standalone: true,
})
export class CollegeLicenceInformationDetailComponent {
  @Input() public labelText = '';
  @Input() public valueText: string | undefined;
}
