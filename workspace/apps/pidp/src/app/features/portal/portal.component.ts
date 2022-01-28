import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { Observable, exhaustMap, map, of } from 'rxjs';

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
  public title: string;
  public acceptedCollectionNotice: boolean;
  public collectionNotice: string;
  public state$: Observable<Record<string, PortalSection[]>>;
  public completedProfile: boolean;

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
    this.completedProfile = route.snapshot.queryParams.profileCompleted;
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
    this.partyResource
      .firstOrCreate()
      .pipe(
        exhaustMap((partyId: number | null) =>
          partyId ? this.partyResource.getPartyProfileStatus(partyId) : of(null)
        ),
        map((profileStatus: ProfileStatus | null) =>
          this.partyService.updateState(profileStatus)
        )
      )
      .subscribe();
  }
}
