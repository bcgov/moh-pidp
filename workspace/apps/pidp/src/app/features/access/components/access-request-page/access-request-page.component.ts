import { NgClass, NgIf } from '@angular/common';
import { Component, Inject, HostListener } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';

import { FaIconComponent } from '@fortawesome/angular-fontawesome';
import { faPlus, faThumbsUp, faFileLines, faArrowUp, faAngleRight, faMagnifyingGlass,faCheck } from '@fortawesome/free-solid-svg-icons';
import { InjectViewportCssClassDirective } from '@bcgov/shared/ui';

import { APP_CONFIG, AppConfig } from '@app/app.config';
import { LayoutHeaderFooterComponent } from "../../../../../../../../libs/shared/ui/src/lib/components/layout-header-footer/layout-header-footer.component";

@Component({
    selector: 'app-access-request-page',
    templateUrl: './access-request-page.component.html',
    styleUrls: ['./access-request-page.component.scss'],
    standalone: true,
    imports: [
        FaIconComponent,
        InjectViewportCssClassDirective,
        MatButtonModule,
        NgClass,
        NgIf,
        LayoutHeaderFooterComponent
    ]
})
export class AccessRequestPageComponent {
  public faThumbsUp = faThumbsUp;
  public faPlus = faPlus;
  public faFileLines = faFileLines;
  public faCheck = faCheck;
  public logoutRedirectUrl: string;
  public faArrowUp = faArrowUp;
  public faAngleRight = faAngleRight;
  public faMagnifyingGlass = faMagnifyingGlass;
  public showBackToTopButton: boolean = false;
  public showSearchIcon: boolean = true;
  public isMobile = true;
  public providerIdentitySupport: string;

  public constructor(
    @Inject(APP_CONFIG) private config: AppConfig,
  ) {
    this.providerIdentitySupport = this.config.emails.providerIdentitySupport;
    this.logoutRedirectUrl = `${this.config.applicationUrl}/`;
  }

  @HostListener('window:scroll', [])
 public onWindowScroll() {
    const scrollPosition = window.scrollY || document.documentElement.scrollTop || document.body.scrollTop || 0;
    const scrollThreshold = 200;
    this.showBackToTopButton = scrollPosition > scrollThreshold;
  }

 public  scrollToTop() {
    window.scrollTo({ top: 0, behavior: 'smooth' });
  }

  public onSearch(event: Event) {
    this.showSearchIcon = ( event.target as HTMLInputElement).value ==='';
  }
 
}
