import { NgClass, NgIf } from '@angular/common';
import { Component, Inject, Input } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { Router } from '@angular/router';

import { FaIconComponent } from '@fortawesome/angular-fontawesome';
import { faPlus, faThumbsUp, faFile,faFileLines,faArrowUp } from '@fortawesome/free-solid-svg-icons';

import { InjectViewportCssClassDirective } from '@bcgov/shared/ui';

import { APP_CONFIG, AppConfig } from '@app/app.config';
import { AuthService } from '@app/features/auth/services/auth.service';

//import { IPortalSection } from '../../state/portal-section.model';

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


  public constructor(
    @Inject(APP_CONFIG) private config: AppConfig,
    private router: Router,
    private authService: AuthService,
  ) {
    this.logoutRedirectUrl = `${this.config.applicationUrl}/`;
  }
  scrollToTop() {
    window.scrollTo({ top: 0, behavior: 'smooth' });
  }

}

 