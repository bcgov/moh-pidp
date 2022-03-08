import { Component, Inject, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { Observable, Subscription, map } from 'rxjs';

import { APP_CONFIG, AppConfig } from '@app/app.config';
import { AccessRequestResource } from '@app/core/resources/access-request-resource.service';
import { DocumentService } from '@app/core/services/document.service';
import { Role } from '@app/shared/enums/roles.enum';

import { PartyService } from '@core/services/party.service';

import { ShellRoutes } from '../shell/shell.routes';
import {
  PortalSection,
  PortalSectionType,
} from './models/portal-section.model';
import {
  ProfileStatus,
  ProfileStatusAlert,
  StatusCode,
} from './models/profile-status.model';
import { PortalResource } from './portal-resource.service';
import { PortalService } from './portal.service';

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
  public alerts: ProfileStatusAlert[];
  public providerIdentitySupportEmail: string;
  public specialAuthoritySupportEmail: string;

  public Role = Role;

  public constructor(
    @Inject(APP_CONFIG) private config: AppConfig,
    private route: ActivatedRoute,
    private router: Router,
    private partyService: PartyService,
    private portalResource: PortalResource,
    private portalService: PortalService,
    private accessRequestResource: AccessRequestResource,
    documentService: DocumentService
  ) {
    this.title = this.route.snapshot.data.title;
    this.acceptedCollectionNotice = this.portalService.acceptedCollectionNotice;
    this.collectionNotice = documentService.getSAeFormsCollectionNotice();
    this.state$ = this.portalService.state$;
    this.completedProfile = false;
    this.alerts = [];
    this.providerIdentitySupportEmail =
      this.config.emails.providerIdentitySupport;
    this.specialAuthoritySupportEmail =
      this.config.emails.specialAuthoritySupport;
  }

  public onAcceptCollectionNotice(accepted: boolean): void {
    this.portalService.acceptedCollectionNotice = accepted;
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

  // TODO drop PortalSectionType and use type narrowing
  public onCardRequestAccess(
    sectionType: PortalSectionType,
    routePath: string
  ): void {
    const partyId = this.partyService.partyId;
    const profileStatus = this.portalService.profileStatus;

    if (!partyId || !profileStatus) {
      return;
    }

    if (sectionType === PortalSectionType.SA_EFORMS) {
      // TODO don't allow access if identity provider is Active Directory and show modal
      const saEformsStatusCode = profileStatus.status.saEforms.statusCode;

      if (saEformsStatusCode !== StatusCode.COMPLETED) {
        this.busy = this.accessRequestResource
          .saEforms(partyId)
          .subscribe(() => {
            if (!routePath) {
              return;
            }

            this.router.navigate([ShellRoutes.routePath(routePath)]);
          });
      } else {
        this.router.navigate([ShellRoutes.routePath(routePath)]);
      }
    } else if (sectionType === PortalSectionType.HCIM_WEB_ENROLMENT) {
      // TODO don't allow access if identity provider is BCSC and show modal
      this.router.navigate([ShellRoutes.routePath(routePath)]);
    }
  }

  public ngOnInit(): void {
    this.busy = this.portalResource
      .getProfileStatus(this.partyService.partyId)
      .pipe(
        map((profileStatus: ProfileStatus | null) => {
          this.portalService.updateState(profileStatus);
          this.completedProfile = this.portalService.completedProfile;
          this.alerts = this.portalService.alerts;
        })
      )
      .subscribe();
  }
}
