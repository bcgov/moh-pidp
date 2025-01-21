import { NgFor, NgIf } from '@angular/common';
import {
  Component,
  HostListener,
  Inject,
  OnDestroy,
  OnInit,
} from '@angular/core';
import { MatButtonModule } from '@angular/material/button';

import { Observable, Subject, map, takeUntil, tap } from 'rxjs';

import { FaIconComponent } from '@fortawesome/angular-fontawesome';
import {
  faArrowUp,
  faMagnifyingGlass,
} from '@fortawesome/free-solid-svg-icons';

import {
  InjectViewportCssClassDirective,
} from '@bcgov/shared/ui';

import { APP_CONFIG, AppConfig } from '@app/app.config';
import { PartyService } from '@app/core/party/party.service';
import { PortalResource } from '@app/features/portal/portal-resource.service';
import { PortalService } from '@app/features/portal/portal.service';
import { IAccessSection } from '@app/features/portal/state/access-section.model';
import { AccessState } from '@app/features/portal/state/portal-state.builder';
import { BreadcrumbComponent } from '@app/shared/components/breadcrumb/breadcrumb.component';
import { Constants } from '@app/shared/constants';

import { AccessRequestCardComponent } from '../../components/access-request-card/access-request-card.component';

@Component({
  selector: 'app-access-request-page',
  templateUrl: './access-requests.page.html',
  styleUrls: ['./access-requests.page.scss'],
  standalone: true,
  imports: [
    BreadcrumbComponent,
    FaIconComponent,
    InjectViewportCssClassDirective,
    MatButtonModule,
    NgIf,
    AccessRequestCardComponent,
    NgFor,
  ],
})
export class AccessRequestsPage implements OnInit, OnDestroy {
  /**
   * @description
   * State for driving the displayed groups and sections of
   * the portal.
   */
  public accessState$: Observable<AccessState>;

  public faArrowUp = faArrowUp;
  public faMagnifyingGlass = faMagnifyingGlass;
  public logoutRedirectUrl: string;
  public showBackToTopButton: boolean = false;
  public showSearchIcon: boolean = true;
  public isMobile = true;
  public providerIdentitySupport: string;
  public filteredAccessSections: IAccessSection[] | undefined = [];
  public breadcrumbsData: Array<{ title: string; path: string }> = [
    { title: 'Home', path: '' },
    { title: 'Access', path: '' },
  ];
  private readonly destroy$ = new Subject<void>();

  public constructor(
    @Inject(APP_CONFIG) private readonly config: AppConfig,
    private readonly partyService: PartyService,
    private readonly portalService: PortalService,
    private readonly portalResource: PortalResource,
  ) {
    this.accessState$ = this.portalService.accessState$;
    this.providerIdentitySupport = this.config.emails.providerIdentitySupport;
    this.logoutRedirectUrl = `${this.config.applicationUrl}/`;
    this.getCards();
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

  public search(text: string): void {
    if (!text) {
      this.getCards();
      return;
    }
    text = text.trim();
    this.accessState$
      .pipe(map((state) => state?.access))
      .subscribe((access) => {
        this.filteredAccessSections = access?.filter(
          (section) =>
            section.heading.toLowerCase().includes(text.toLowerCase()) ||
            section.description.toLowerCase().includes(text.toLowerCase()) ||
            section.keyWords?.includes(text.toLocaleLowerCase())
        );
      });
  }

  public onSearch(event: Event): void {
    this.showSearchIcon = (event.target as HTMLInputElement).value === '';
    if (this.showSearchIcon) {
      this.getCards();
    }
  }

  public onCardAction(section: IAccessSection): void {
    section.performAction();
  }

  public ngOnInit(): void {
    this.portalResource
      .getProfileStatus(this.partyService.partyId)
      .pipe(
        tap((profileStatus) => this.portalService.updateState(profileStatus)),
        takeUntil(this.destroy$),
      )
      .subscribe();
  }

  public ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  private getCards(): void {
    this.accessState$
      .pipe(
        map((state) => state?.access),
        takeUntil(this.destroy$),
      )
      .subscribe((access) => {
        this.filteredAccessSections = access;
        this.filteredAccessSections?.sort((a, b) =>
          a.heading.localeCompare(b.heading),
        );
      });
  }
}
