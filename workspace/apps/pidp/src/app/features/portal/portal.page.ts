import { STEPPER_GLOBAL_OPTIONS } from '@angular/cdk/stepper';
import { NgIf } from '@angular/common';
import { Component, Inject, OnInit, HostListener } from '@angular/core';

import { FaIconComponent } from '@fortawesome/angular-fontawesome';
import { faArrowAltCircleRight  } from '@fortawesome/free-regular-svg-icons';
import {  faArrowUp, faArrowAltCircleUp} from '@fortawesome/free-solid-svg-icons';
import {
  InjectViewportCssClassDirective
} from '@bcgov/shared/ui';

import { APP_CONFIG, AppConfig } from '@app/app.config';



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
    NgIf
  ],
})
export class PortalPage implements OnInit {
 
  public faArrowAltCircleRight = faArrowAltCircleRight;
  public faArrowUp = faArrowUp;
  public faArrowAltCircleUp = faArrowAltCircleUp;
  public showBackToTopButton: boolean = false;

  public constructor(
    @Inject(APP_CONFIG) private config: AppConfig,
    
  ) {
     
  }

  public ngOnInit(): void {
    
  }
 
  @HostListener('window:scroll', [])
  public onWindowScroll() {
    const scrollPosition = window.scrollY || document.documentElement.scrollTop || document.body.scrollTop || 0;
    const scrollThreshold = 200;
    this.showBackToTopButton = scrollPosition > scrollThreshold;
    return;
  }
  public  scrollToTop() {
    window.scrollTo({ top: 0, behavior: 'smooth' });
    return;
  }
}
