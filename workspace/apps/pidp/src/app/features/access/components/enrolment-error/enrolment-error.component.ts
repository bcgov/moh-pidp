import { Component } from '@angular/core';

import { AlertComponent } from '@bcgov/shared/ui';

@Component({
  selector: 'app-enrolment-error',
  template: `
    <ui-alert
      type="info"
      icon="error_outline"
      iconType="outlined"
      heading="Error">
      <ng-container uiAlertContent>
        <p>There was an error with this enrolment, try again later.</p>
      </ng-container>
    </ui-alert>
  `,
  styles: [],
  standalone: true,
  imports: [AlertComponent],
})
export class EnrolmentErrorComponent {}
