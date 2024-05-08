import { NgClass, NgIf } from '@angular/common';
import { Component, Inject, HostListener, Input } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';

import { FaIconComponent } from '@fortawesome/angular-fontawesome';
import { faPlus, faThumbsUp, faFileLines, faArrowUp, faAngleRight, faMagnifyingGlass, faL } from '@fortawesome/free-solid-svg-icons';
import { InjectViewportCssClassDirective } from '@bcgov/shared/ui';

import { APP_CONFIG, AppConfig } from '@app/app.config';

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
  ],
})
export class AccessRequestPageComponent {
  public faThumbsUp = faThumbsUp;
  public faPlus = faPlus;
  public faFileLines = faFileLines;
  public logoutRedirectUrl: string;
  public faArrowUp = faArrowUp;
  public faAngleRight = faAngleRight;
  public faMagnifyingGlass = faMagnifyingGlass;
  showBackToTopButton: Boolean = false;
  showSearrchIcon: boolean = true;

  public constructor(
    @Inject(APP_CONFIG) private config: AppConfig,
  ) {
    this.logoutRedirectUrl = `${this.config.applicationUrl}/`;
  }

  @HostListener('window:scroll', [])
  onWindowScroll() {
    const scrollPosition = window.scrollY || document.documentElement.scrollTop || document.body.scrollTop || 0;
    const scrollThreshold = 200;
    this.showBackToTopButton = scrollPosition > scrollThreshold;
  }

  scrollToTop() {
    window.scrollTo({ top: 0, behavior: 'smooth' });
  }

  onSearch(event: KeyboardEvent) {
    this.showSearrchIcon = (<HTMLInputElement>event.target).value == '';
  }
}
