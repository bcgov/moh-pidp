import { Component, HostListener, Inject, OnInit, signal } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { ActivatedRoute, Router } from '@angular/router';
import {MatExpansionModule} from '@angular/material/expansion';
import { InjectViewportCssClassDirective } from '@bcgov/shared/ui';

import {
  AnchorDirective,
  PageComponent,
  PageFooterActionDirective,
  PageFooterComponent,
  PageHeaderComponent,
  PageSectionComponent,
  PageSectionSubheaderComponent,
} from '@bcgov/shared/ui';

import { APP_CONFIG, AppConfig } from '@app/app.config';
import { UtilsService } from '@app/core/services/utils.service';
import { faAngleRight, faArrowUp } from '@fortawesome/free-solid-svg-icons';
import { FaIconComponent } from '@fortawesome/angular-fontawesome';
import { FaqRoutes } from '../../faq.routes';
import { Constants } from '@app/shared/constants';
import { NgIf } from '@angular/common';

@Component({
  selector: 'app-help',
  templateUrl: './help.page.html',
  styleUrls: ['./help.page.scss'],
  standalone: true,
  imports: [
    AnchorDirective,
    FaIconComponent,
    MatButtonModule,
    MatExpansionModule,
    NgIf,
    PageComponent,
    PageFooterActionDirective,
    PageFooterComponent,
    PageHeaderComponent,
    PageSectionComponent,
    PageSectionSubheaderComponent,
    InjectViewportCssClassDirective,
  ],
})
export class HelpPage implements OnInit {
  public providerIdentitySupport: string;
  public readonly panelOpenState = signal(false);
  public showBackToTopButton: boolean = false;
  public faArrowUp = faArrowUp;
  public faAngleRight = faAngleRight;

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
     const scrollPosition = window.scrollY || document.documentElement.scrollTop || document.body.scrollTop || 0;
     this.showBackToTopButton = scrollPosition > Constants.scrollThreshold;
   }

 public  scrollToTop(): void {
  window.scrollTo({ top: 0, behavior: 'smooth' });
}

  public onBack(): void {
    this.navigateToRoot();
  }

  public ngOnInit(): void {
    this.utilsService.scrollTop();
  }

  public onClickSendEmail(address: string): void {
    window.location.assign(address);
  }

  public navigateToMfaPage(): void {
    this.router.navigateByUrl(FaqRoutes.routePath(FaqRoutes.MFA_SETUP));
  }

  private navigateToRoot(): void {
    this.router.navigate([this.route.snapshot.data.routes.root]);
  }
}
