import { Component, OnInit } from '@angular/core';
import { SafeHtml } from '@angular/platform-browser';
import { ActivatedRoute, Router } from '@angular/router';

import { exhaustMap, of } from 'rxjs';

import { AlertType } from '@bcgov/shared/ui';

import { DocumentService } from '@app/core/services/document.service';
import { Role } from '@app/shared/enums/roles.enum';

import { PartyResource } from '@core/resources/party-resource.service';
import { PartyService } from '@core/services/party.service';

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
  public collectionNotice: SafeHtml;

  public Role = Role;

  public constructor(
    private route: ActivatedRoute,
    private router: Router,
    private partyService: PartyService,
    private partyResource: PartyResource,
    documentService: DocumentService
  ) {
    this.title = this.route.snapshot.data.title;

    this.acceptedCollectionNotice = this.partyService.acceptedCollectionNotice;
    this.completedProfile = this.partyService.completedProfile;
    this.state = this.partyService.state;
    this.collectionNotice = documentService.getCollectionNotice();
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
    // TODO merge profile status into the default card statuses
    this.partyResource
      .firstOrCreate()
      .pipe(
        exhaustMap((partyId: number | null) =>
          partyId ? this.partyResource.getPartyProfileStatus(partyId) : of(null)
        )
      )
      .subscribe(console.log);
  }
}
