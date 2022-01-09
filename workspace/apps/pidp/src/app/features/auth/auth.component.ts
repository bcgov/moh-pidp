import { Component } from '@angular/core';

import { DashboardHeaderTheme } from '@bcgov/shared/ui';

@Component({
  selector: 'app-auth',
  templateUrl: './auth.component.html',
  styleUrls: ['./auth.component.scss'],
})
export class AuthComponent {
  public theme: DashboardHeaderTheme;

  public constructor() {
    this.theme = 'light';
  }

  public onLogin(): void {
    // TODO not implemented
  }
}
