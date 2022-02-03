import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { Observable, Subscription, exhaustMap, map, of } from 'rxjs';

import { DocumentService } from '@app/core/services/document.service';
import { Role } from '@app/shared/enums/roles.enum';

import {
  PartyResource,
  ProfileStatus,
} from '@core/resources/party-resource.service';
import { PartyService } from '@core/services/party.service';

import { ShellRoutes } from '../shell/shell.routes';
import { PortalSection } from './models/portal-section.model';

@Component({
  selector: 'app-portal',
  templateUrl: './portal.component.html',
  styleUrls: ['./portal.component.scss'],
})
export class PortalComponent implements OnInit {
  public busy?: Subscription;
  public title: string;
  public acceptedCollectionNotice: boolean;
  public collectionNotice: string;
  public state$: Observable<Record<string, PortalSection[]>>;
  public hasCompletedProfile: boolean;
  public completedProfile: boolean;
  public collegeLicenceVerified: boolean;

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
    this.collectionNotice = documentService.getCollectionNotice();
    this.state$ = this.partyService.state$;
    // TODO won't scale better as notification service that's set until displayed
    //      and uses an accompanying component since there are already 3 different
    //      notifications for SA eForms
    this.hasCompletedProfile =
      route.snapshot.queryParams.completedProfile === 'true';
    this.completedProfile = this.partyService.completedProfile;
    this.collegeLicenceVerified = this.partyService.collegeLicenceVerified;
  }

  public onAcceptCollectionNotice(accepted: boolean): void {
    this.partyService.acceptedCollectionNotice = accepted;
  }

  public onScrollToAnchor(): void {
    this.router.navigate([], {
      fragment: 'access',
      queryParamsHandling: 'preserve',
    });
  }

  public onCardRouteAction(routePath?: string): void {
    if (!routePath) {
      return;
    }

    this.router.navigate([ShellRoutes.routePath(routePath)]);
  }

  public ngOnInit(): void {
    this.busy = this.partyResource
      .firstOrCreate()
      .pipe(
        exhaustMap((partyId: number | null) =>
          partyId ? this.partyResource.getProfileStatus(partyId) : of(null)
        ),
        // TODO instantiate profile status to get access to helper methods
        // map((profileStatus: ProfileStatus | null) => new ProfileStatus()),
        map((profileStatus: ProfileStatus | null) =>
          this.partyService.updateState(profileStatus)
        )
      )
      .subscribe();
  }
}
