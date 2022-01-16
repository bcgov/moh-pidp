import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { AlertType } from '@bcgov/shared/ui';

import { PartyResource } from '@core/resources/party-resource.service';
import { PartyService } from '@core/services/party.service';

import { collectionNotice } from '@shared/data/collection-notice.data';

import { ShellRoutes } from '../shell/shell.routes';

export interface PortalSection {
  icon: string;
  type: string;
  title: string;
  description: string;
  process: 'manual' | 'automatic';
  hint?: string;
  actionLabel?: string;
  route?: string;
  statusType?: AlertType;
  status?: string;
  disabled: boolean;
}

// TODO find a clean way to type narrow in the template
@Component({
  selector: 'app-portal',
  templateUrl: './portal.component.html',
  styleUrls: ['./portal.component.scss'],
})
export class PortalComponent implements OnInit {
  public title: string;
  public acceptedCollectionNotice: boolean;
  public state: Record<string, PortalSection[]>;
  public completedProfile: boolean;
  public collectionNotice: string;

  public constructor(
    private route: ActivatedRoute,
    private router: Router,
    private partyService: PartyService,
    private partyResource: PartyResource
  ) {
    this.title = this.route.snapshot.data.title;

    this.acceptedCollectionNotice = this.partyService.acceptedCollectionNotice;
    this.completedProfile = this.partyService.completedProfile;
    this.state = this.partyService.state;
    this.collectionNotice = collectionNotice;
  }

  public onAcceptCollectionNotice(accepted: boolean): void {
    this.partyService.acceptedCollectionNotice = accepted;
  }

  public onAction(routePath?: string): void {
    if (!routePath) {
      return;
    }

    this.router.navigate([ShellRoutes.routePath(routePath)]);
  }

  public ngOnInit(): void {
    this.partyResource.firstOrCreate().subscribe(console.log);
  }
}
