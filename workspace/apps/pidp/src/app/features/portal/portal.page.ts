import { STEPPER_GLOBAL_OPTIONS } from '@angular/cdk/stepper';
import { AsyncPipe, NgIf, NgOptimizedImage } from '@angular/common';
import { Component, HostListener } from '@angular/core';
import { Router } from '@angular/router';

import { Observable, map } from 'rxjs';

import { FaIconComponent } from '@fortawesome/angular-fontawesome';
import { faBell } from '@fortawesome/free-regular-svg-icons';
import { faArrowRight, faArrowUp } from '@fortawesome/free-solid-svg-icons';

import { InjectViewportCssClassDirective } from '@bcgov/shared/ui';

import { PartyService } from '@app/core/party/party.service';

import { AccessRoutes } from '../access/access.routes';
import { OrganizationInfoRoutes } from '../organization-info/organization-info.routes';
import { ProfileRoutes } from '../profile/profile.routes';
import { AlertCode } from './enums/alert-code.enum';
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
    AsyncPipe,
  ],
})
export class PortalPage {
  public faArrowRight = faArrowRight;
  public faArrowUp = faArrowUp;
  public showBackToTopButton: boolean = false;
  public ProfileRoutes = ProfileRoutes;
  public AccessRoutes = AccessRoutes;
  public OrganizationInfoRoutes = OrganizationInfoRoutes;
  public AlertCode = AlertCode;
  public faBell = faBell;
  public profileStatusAlerts$: Observable<AlertCode[]>;

  public constructor(
    private partyService: PartyService,
    private resource: PortalResource,
    private router: Router,
  ) {
    this.profileStatusAlerts$ = this.resource
      .getProfileStatus(this.partyService.partyId)
      .pipe(map((profileStatus) => profileStatus?.alerts ?? []));
  }

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
}
