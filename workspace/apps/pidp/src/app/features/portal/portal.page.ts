import { STEPPER_GLOBAL_OPTIONS } from '@angular/cdk/stepper';
import { NgIf, NgOptimizedImage } from '@angular/common';
import { Component, HostListener, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { map } from 'rxjs';

import { FaIconComponent } from '@fortawesome/angular-fontawesome';
import { faBell } from '@fortawesome/free-regular-svg-icons';
import { faArrowRight, faArrowUp } from '@fortawesome/free-solid-svg-icons';

import { InjectViewportCssClassDirective } from '@bcgov/shared/ui';

import { PartyService } from '@app/core/party/party.service';

import { AccessRoutes } from '../access/access.routes';
import { OrganizationInfoRoutes } from '../organization-info/organization-info.routes';
import { ProfileRoutes } from '../profile/profile.routes';
import { AlertCode } from './enums/alert-code.enum';
import { ProfileStatus } from './models/profile-status.model';
import { PortalResource } from './portal-resource.service';

@Component({
  selector: 'app-portal',
  templateUrl: './portal.page.html',
  styleUrls: ['./portal.page.scss'],
  providers: [
    {
      provide: STEPPER_GLOBAL_OPTIONS,
      useValue: { displayDefaultIndicatorType: false },
    },
  ],
  standalone: true,
  imports: [
    FaIconComponent,
    InjectViewportCssClassDirective,
    NgIf,
    NgOptimizedImage,
  ],
})
export class PortalPage implements OnInit {
  public faArrowRight = faArrowRight;
  public faArrowUp = faArrowUp;
  public showBackToTopButton: boolean = false;
  public ProfileRoutes = ProfileRoutes;
  public AccessRoutes = AccessRoutes;
  public OrganizationInfoRoutes = OrganizationInfoRoutes;
  public alerts: AlertCode[] = [];
  public AlertCode = AlertCode;
  public faBell = faBell;

  public constructor(
    private partyService: PartyService,
    private resource: PortalResource,
    private router: Router,
  ) {}

  @HostListener('window:scroll', [])
  public onWindowScroll(): void {
    const scrollPosition =
      window.scrollY ||
      document.documentElement.scrollTop ||
      document.body.scrollTop ||
      0;
    const scrollThreshold = 200;
    this.showBackToTopButton = scrollPosition > scrollThreshold;
  }

  public scrollToTop(): void {
    window.scrollTo({ top: 0, behavior: 'smooth' });
  }

  public navigateTo(path: string): void {
    this.router.navigateByUrl(path);
  }

  public ngOnInit(): void {
    const partyId = this.partyService.partyId;
    this.resource
      .getProfileStatus(partyId)
      .pipe(
        map(
          (profileStatus: ProfileStatus | null) =>
            (this.alerts = profileStatus?.alerts ?? []),
        ),
      )
      .subscribe();
  }
}
