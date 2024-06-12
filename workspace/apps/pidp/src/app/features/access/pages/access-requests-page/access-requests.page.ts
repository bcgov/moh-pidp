import { AsyncPipe, NgClass, NgFor, NgIf } from '@angular/common';
import { Component, HostListener, Inject, OnInit } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';

import { Observable, tap } from 'rxjs';

import { FaIconComponent } from '@fortawesome/angular-fontawesome';
import {
  faAngleRight,
  faArrowUp,
  faArrowsLeftRightToLine,
  faBookMedical,
  faCar,
  faCheck,
  faFileLines,
  faFileMedical,
  faHandshake,
  faLink,
  faMagnifyingGlass,
  faPeopleGroup,
  faPlus,
  faRightLeft,
  faThumbsUp,
} from '@fortawesome/free-solid-svg-icons';
import { NavigationService } from '@pidp/presentation';

import {
  InjectViewportCssClassDirective,
  LayoutHeaderFooterComponent,
  TextButtonDirective,
} from '@bcgov/shared/ui';

import { APP_CONFIG, AppConfig } from '@app/app.config';
import { PartyService } from '@app/core/party/party.service';
import { PortalResource } from '@app/features/portal/portal-resource.service';
import { PortalService } from '@app/features/portal/portal.service';
import { AccessState } from '@app/features/portal/state/portal-state.builder';
import { Constants } from '@app/shared/constants';

import { AccessRequestCardComponent } from '../../components/access-request-card/access-request-card.component';

@Component({
  selector: 'app-access-request-page',
  templateUrl: './access-requests.page.html',
  styleUrls: ['./access-requests.page.scss'],
  standalone: true,
  imports: [
    FaIconComponent,
    InjectViewportCssClassDirective,
    MatButtonModule,
    NgClass,
    NgIf,
    LayoutHeaderFooterComponent,
    TextButtonDirective,
    AccessRequestCardComponent,
    NgFor,
    AsyncPipe,
  ],
})
export class AccessRequestsPage implements OnInit {
  /**
   * @description
   * State for driving the displayed groups and sections of
   * the portal.
   */
  public accessState$: Observable<AccessState>;

  public faThumbsUp = faThumbsUp;
  public faPlus = faPlus;
  public faFileLines = faFileLines;
  public faCheck = faCheck;
  public logoutRedirectUrl: string;
  public faArrowUp = faArrowUp;
  public faAngleRight = faAngleRight;
  public faMagnifyingGlass = faMagnifyingGlass;
  public faHandshake = faHandshake;
  public faLink = faLink;
  public faCar = faCar;
  public faFileMedical = faFileMedical;
  public faBookMedical = faBookMedical;
  public faArrowsLeftRightToLine = faArrowsLeftRightToLine;
  public faPeopleGroup = faPeopleGroup;
  public faRightLeft = faRightLeft;
  public showBackToTopButton: boolean = false;
  public showSearchIcon: boolean = true;
  public isMobile = true;
  public providerIdentitySupport: string;

  public constructor(
    @Inject(APP_CONFIG) private config: AppConfig,
    private navigationService: NavigationService,
    private partyService: PartyService,
    private portalService: PortalService,
    private portalResource: PortalResource,
  ) {
    this.accessState$ = this.portalService.accessState$;
    this.providerIdentitySupport = this.config.emails.providerIdentitySupport;
    this.logoutRedirectUrl = `${this.config.applicationUrl}/`;
  }

  @HostListener('window:scroll', [])
  public onWindowScroll(): void {
    const scrollPosition =
      window.scrollY ||
      document.documentElement.scrollTop ||
      document.body.scrollTop ||
      0;
    this.showBackToTopButton = scrollPosition > Constants.scrollThreshold;
  }

  public scrollToTop(): void {
    window.scrollTo({ top: 0, behavior: 'smooth' });
  }

  public onSearch(event: Event): void {
    this.showSearchIcon = (event.target as HTMLInputElement).value === '';
  }

  public onBack(): void {
    this.navigationService.navigateToRoot();
  }

  public ngOnInit(): void {
    this.portalResource
      .getProfileStatus(this.partyService.partyId)
      .pipe(
        tap((profileStatus) => this.portalService.updateState(profileStatus)),
      )
      .subscribe();
  }
}
