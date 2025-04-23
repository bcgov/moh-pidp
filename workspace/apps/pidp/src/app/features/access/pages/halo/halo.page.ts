import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatStepperModule } from '@angular/material/stepper';
import { Router } from '@angular/router';

import { BehaviorSubject, Observable } from 'rxjs';

import { InjectViewportCssClassDirective } from '@bcgov/shared/ui';

import {
  Destination,
  DiscoveryResource,
} from '@app/core/party/discovery-resource.service';
import { PartyService } from '@app/core/party/party.service';
import { BreadcrumbComponent } from '@app/shared/components/breadcrumb/breadcrumb.component';

import { AccessRoutes } from '../../access.routes';
import { bcProviderTutorialLink } from '../provincial-attachment-system/provincial-attachment-system.constants';

@Component({
  selector: 'app-halo',
  standalone: true,
  imports: [
    CommonModule,
    MatButtonModule,
    MatIconModule,
    BreadcrumbComponent,
    MatStepperModule,
    InjectViewportCssClassDirective,
  ],
  templateUrl: './halo.page.html',
  styleUrl: './halo.page.scss',
})
export class HaloPage {
  public bcProvider$: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(
    false,
  );
  public destination$: Observable<Destination>;
  public halo$: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(true);
  public hasCpn: boolean | undefined;
  public selectedIndex: number;
  public bcProviderTutorial: string;
  public Destination = Destination;

  public breadcrumbsData: Array<{ title: string; path: string }> = [
    { title: 'Home', path: '' },
    {
      title: 'Access',
      path: AccessRoutes.routePath(AccessRoutes.ACCESS_REQUESTS),
    },
    { title: 'PAS', path: '' },
  ];

  public constructor(
    private discoveryResource: DiscoveryResource,
    private partyService: PartyService,
    private router: Router,
  ) {
    this.selectedIndex = -1;
    this.bcProviderTutorial = bcProviderTutorialLink;
    this.destination$ = this.discoveryResource.getDestination(
      this.partyService.partyId,
    );
  }

  public navigateToPath(): void {
    this.router.navigate(['/']);
  }
}
