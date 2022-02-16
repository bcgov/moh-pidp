import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { Observable, Subscription, exhaustMap, map, of } from 'rxjs';

import { AccessRequestResource } from '@app/core/resources/access-request-resource.service';
import { DocumentService } from '@app/core/services/document.service';
import { Role } from '@app/shared/enums/roles.enum';

import {
  PartyResource,
  ProfileStatus,
  StatusCode,
} from '@core/resources/party-resource.service';
import { PartyService } from '@core/services/party.service';

import { ShellRoutes } from '../shell/shell.routes';
import { PortalSection } from './models/portal-section.model';

@Component({
  selector: 'app-portal',
  templateUrl: './portal.page.html',
  styleUrls: ['./portal.page.scss'],
})
export class PortalPage implements OnInit {
  public busy?: Subscription;
  public title: string;
  public acceptedCollectionNotice: boolean;
  public collectionNotice: string;
  public state$: Observable<Record<string, PortalSection[]>>;
  public completedProfile: boolean;
  public collegeLicenceValidationError: boolean;

  public Role = Role;

  public constructor(
    private route: ActivatedRoute,
    private router: Router,
    private partyService: PartyService,
    private partyResource: PartyResource,
    private accessRequestResource: AccessRequestResource,
    documentService: DocumentService
  ) {
    this.title = this.route.snapshot.data.title;
    this.acceptedCollectionNotice = this.partyService.acceptedCollectionNotice;
    this.collectionNotice = documentService.getCollectionNotice();
    this.state$ = this.partyService.state$;
    this.completedProfile = false;
    this.collegeLicenceValidationError = false;
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

  public onCardRouteAction(routePath: string): void {
    if (!routePath) {
      return;
    }

    this.router.navigate([ShellRoutes.routePath(routePath)]);
  }

  public onCardRequestAccess(routePath: string): void {
    // TODO remove possibility of profileStatus being empty from type
    const profileStatus = this.partyService.profileStatus;
    const partyId = profileStatus?.id;

    if (!partyId) {
      return;
    }

    const saEformsStatusCode = profileStatus.status.saEforms.statusCode;

    if (saEformsStatusCode !== StatusCode.COMPLETED) {
      this.busy = this.accessRequestResource.saEforms(partyId).subscribe(() => {
        if (!routePath) {
          return;
        }

        this.router.navigate([ShellRoutes.routePath(routePath)]);
      });
    } else {
      this.router.navigate([ShellRoutes.routePath(routePath)]);
    }
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
        map((profileStatus: ProfileStatus | null) => {
          this.partyService.updateState(profileStatus);
          this.completedProfile = this.partyService.completedProfile;
          this.collegeLicenceValidationError =
            this.partyService.collegeLicenceValidationError;
        })
      )
      .subscribe();
  }
}
