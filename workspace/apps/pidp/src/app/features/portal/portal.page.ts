import { STEPPER_GLOBAL_OPTIONS } from '@angular/cdk/stepper';
import { AsyncPipe, NgOptimizedImage } from '@angular/common';
import { AfterViewInit, Component, HostListener, OnInit, inject } from '@angular/core';
import { Router } from '@angular/router';

import { Observable, map } from 'rxjs';

import { FaIconComponent } from '@fortawesome/angular-fontawesome';
import { faBell } from '@fortawesome/free-regular-svg-icons';
import { faArrowUp, faBookmark } from '@fortawesome/free-solid-svg-icons';

import { InjectViewportCssClassDirective } from '@bcgov/shared/ui';

import { PartyService } from '@app/core/party/party.service';
import { SnowplowService } from '@app/core/services/snowplow.service';

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
  imports: [
    FaIconComponent,
    InjectViewportCssClassDirective,
    NgOptimizedImage,
    AsyncPipe
],
})
export class PortalPage implements OnInit, AfterViewInit {
  private partyService = inject(PartyService);
  private resource = inject(PortalResource);
  private router = inject(Router);
  private snowplowService = inject(SnowplowService);

  public faBookmark = faBookmark;
  public faArrowUp = faArrowUp;
  public showBackToTopButton = false;
  public ProfileRoutes = ProfileRoutes;
  public AccessRoutes = AccessRoutes;
  public OrganizationInfoRoutes = OrganizationInfoRoutes;
  public AlertCode = AlertCode;
  public faBell = faBell;
  public alerts$!: Observable<AlertCode[]>;

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
    this.alerts$ = this.resource
      .getProfileStatus(this.partyService.partyId)
      .pipe(map((profileStatus) => profileStatus?.alerts ?? []));
  }

  public ngAfterViewInit(): void {
    // refresh link urls now that we set the links
    this.snowplowService.refreshLinkClickTracking();
  }
}
