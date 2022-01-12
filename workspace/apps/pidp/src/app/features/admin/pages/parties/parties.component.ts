import { Component, OnInit } from '@angular/core';

import { DashboardHeaderTheme } from '@bcgov/shared/ui';

@Component({
  selector: 'app-parties',
  templateUrl: './parties.component.html',
  styleUrls: ['./parties.component.scss'],
})
export class PartiesComponent implements OnInit {
  public theme: DashboardHeaderTheme;

  public constructor() {
    this.theme = 'dark';
  }

  public ngOnInit(): void {}
}
