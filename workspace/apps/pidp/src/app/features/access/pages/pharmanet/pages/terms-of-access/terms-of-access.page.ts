import { Component } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { ActivatedRoute, RouterLink } from '@angular/router';

import { SharedUiModule } from '@bcgov/shared/ui';

@Component({
  selector: 'app-terms-of-access',
  templateUrl: './terms-of-access.page.html',
  styleUrls: ['./terms-of-access.page.scss'],
  standalone: true,
  imports: [SharedUiModule, MatCheckboxModule, MatButtonModule, RouterLink],
})
export class TermsOfAccessPage {
  public title: string;

  public constructor(private route: ActivatedRoute) {
    this.title = this.route.snapshot.data.title;
  }
}
