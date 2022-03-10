import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, NavigationExtras, Router } from '@angular/router';

import { PartyService } from '@app/core/party/party.service';
import { LoggerService } from '@app/core/services/logger.service';

import { saEformsUrl } from './special-authority.constants';

@Component({
  selector: 'app-special-authority-eforms',
  templateUrl: './special-authority-eforms.page.html',
  styleUrls: ['./special-authority-eforms.page.scss'],
})
export class SpecialAuthorityEformsPage implements OnInit {
  public title: string;
  public saEformsUrl: string;

  public constructor(
    private route: ActivatedRoute,
    private router: Router,
    private partyService: PartyService,
    private logger: LoggerService
  ) {
    this.title = this.route.snapshot.data.title;
    this.saEformsUrl = saEformsUrl;
  }

  public onBack(): void {
    this.navigateToRoot();
  }

  public ngOnInit(): void {
    const partyId = this.partyService.partyId;
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
