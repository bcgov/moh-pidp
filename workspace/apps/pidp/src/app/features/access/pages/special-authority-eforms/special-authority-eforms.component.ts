import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, NavigationExtras, Router } from '@angular/router';

import { LoggerService } from '@app/core/services/logger.service';
import { PartyService } from '@app/core/services/party.service';

@Component({
  selector: 'app-special-authority-eforms',
  templateUrl: './special-authority-eforms.component.html',
  styleUrls: ['./special-authority-eforms.component.scss'],
})
export class SpecialAuthorityEformsComponent implements OnInit {
  public title: string;
  public saEformsUrl: string;

  public constructor(
    private route: ActivatedRoute,
    private router: Router,
    private partyService: PartyService,
    private logger: LoggerService
  ) {
    this.title = this.route.snapshot.data.title;
    this.saEformsUrl = 'https://www.eforms.phsahealth.ca/appdash';
  }

  public onBack(): void {
    this.navigateToRoot();
  }

  public ngOnInit(): void {
    const partyId = this.partyService.profileStatus?.id;
    if (!partyId) {
      this.logger.error('No party ID was provided');
      return this.navigateToRoot();
    }
  }

  private navigateToRoot(navigationExtras?: NavigationExtras): void {
    this.router.navigate(
      [this.route.snapshot.data.routes.root],
      navigationExtras
    );
  }
}
