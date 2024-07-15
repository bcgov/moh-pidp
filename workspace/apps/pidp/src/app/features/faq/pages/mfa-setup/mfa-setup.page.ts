import { NgIf } from '@angular/common';
import { Component, HostListener, Inject, OnInit } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { ActivatedRoute, Router } from '@angular/router';

import { FaIconComponent } from '@fortawesome/angular-fontawesome';
import { faAngleRight, faArrowUp } from '@fortawesome/free-solid-svg-icons';

import {
  AnchorDirective,
  InjectViewportCssClassDirective,
  PageComponent,
  PageFooterActionDirective,
  PageFooterComponent,
  PageHeaderComponent,
  PageSectionComponent,
  PageSectionSubheaderComponent,
  TextButtonDirective,
} from '@bcgov/shared/ui';

import { APP_CONFIG, AppConfig } from '@app/app.config';
import { UtilsService } from '@app/core/services/utils.service';
import { AccessRoutes } from '@app/features/access/access.routes';
import { Constants } from '@app/shared/constants';

import { FaqRoutes } from '../../faq.routes';

@Component({
  selector: 'app-mfa-setup',
  templateUrl: './mfa-setup.page.html',
  styleUrls: ['./mfa-setup.page.scss'],
  standalone: true,
  imports: [
    AnchorDirective,
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
    TextButtonDirective,
  ],
})
export class MfaSetupPage implements OnInit {
  public providerIdentitySupport: string;
  public faAngleRight = faAngleRight;
  public faArrowUp = faArrowUp;
  public showBackToTopButton: boolean = false;
  public AccessRoutes = AccessRoutes;
  public FaqRoutes = FaqRoutes;

  public constructor(
    @Inject(APP_CONFIG) private config: AppConfig,
    private route: ActivatedRoute,
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

  public onBack(): void {
    this.navigateToRoot();
  }
  public navigateTo(path: string): void {
    this.router.navigateByUrl(path);
  }

  public ngOnInit(): void {
    this.utilsService.scrollTop();
  }

  private navigateToRoot(): void {
    this.router.navigate([this.route.snapshot.data.routes.root]);
  }
}
