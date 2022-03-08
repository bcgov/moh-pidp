import { Component, Inject, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { Observable, Subscription, isObservable, map } from 'rxjs';

import { APP_CONFIG, AppConfig } from '@app/app.config';
import { DocumentService } from '@app/core/services/document.service';
import { PartyService } from '@app/core/services/party.service';
import { Role } from '@app/shared/enums/roles.enum';

import { IPortalSection } from './models/portal-section';
import { ProfileStatusAlert } from './models/profile-status-alert.model';
import { ProfileStatus } from './models/profile-status.model';
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
  public state$: Observable<Record<string, IPortalSection[]>>;
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

  public onCardAction(section: IPortalSection): void {
    const result = section.performAction();
    if (isObservable(result)) {
      this.busy = result.subscribe();
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
