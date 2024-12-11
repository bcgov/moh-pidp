import { NgIf } from '@angular/common';
import { Component, HostListener, Inject, OnInit } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { Router } from '@angular/router';

import { FaIconComponent } from '@fortawesome/angular-fontawesome';
import { faArrowUp } from '@fortawesome/free-solid-svg-icons';

import {
  AnchorDirective,
  InjectViewportCssClassDirective,
  PageComponent,
  PageFooterActionDirective,
  PageFooterComponent,
  PageHeaderComponent,
  PageSectionComponent,
  PageSectionSubheaderComponent,
} from '@bcgov/shared/ui';

import { APP_CONFIG, AppConfig } from '@app/app.config';
import { UtilsService } from '@app/core/services/utils.service';
import { AccessRoutes } from '@app/features/access/access.routes';
import { Constants } from '@app/shared/constants';

import { FaqRoutes } from '../../faq.routes';
import { BreadcrumbComponent } from '@app/shared/components/breadcrumb/breadcrumb.component';
import { FeedbackButtonComponent } from "../../../../shared/components/feedback-button/feedback-button.component";

@Component({
  selector: 'app-mfa-setup',
  templateUrl: './mfa-setup.page.html',
  styleUrls: ['./mfa-setup.page.scss'],
  standalone: true,
  imports: [
    AnchorDirective,
    BreadcrumbComponent,
    FaIconComponent,
    MatButtonModule,
    NgIf,
    PageComponent,
    PageFooterActionDirective,
    PageFooterComponent,
    PageHeaderComponent,
    PageSectionComponent,
    PageSectionSubheaderComponent,
    InjectViewportCssClassDirective,
    FeedbackButtonComponent
],
})
export class MfaSetupPage implements OnInit {
  public providerIdentitySupport: string;
  public faArrowUp = faArrowUp;
  public showBackToTopButton: boolean = false;
  public AccessRoutes = AccessRoutes;
  public FaqRoutes = FaqRoutes;
  public breadcrumbsData: Array<{ title: string; path: string }> = [
    { title: 'Home', path: '' },
    {
      title: 'Help',
      path: FaqRoutes.BASE_PATH,
    },
    { title: 'MFA', path: '' },
  ];

  public constructor(
    @Inject(APP_CONFIG) private config: AppConfig,
    private router: Router,
    private utilsService: UtilsService,
  ) {
    this.providerIdentitySupport = this.config.emails.providerIdentitySupport;
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

  public navigateTo(path: string): void {
    this.router.navigateByUrl(path);
  }

  public ngOnInit(): void {
    this.utilsService.scrollTop();
  }
}
