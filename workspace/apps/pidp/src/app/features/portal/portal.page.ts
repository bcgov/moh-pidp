import { STEPPER_GLOBAL_OPTIONS } from '@angular/cdk/stepper';
import { Component, Inject, OnInit } from '@angular/core';

import { FaIconComponent } from '@fortawesome/angular-fontawesome';
import { faArrowAltCircleRight  } from '@fortawesome/free-regular-svg-icons';

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
    InjectViewportCssClassDirective
  ],
})
export class PortalPage implements OnInit {
 
  public faArrowAltCircleRight = faArrowAltCircleRight;

  public constructor(
    @Inject(APP_CONFIG) private config: AppConfig,
    
  ) {
     
  }

  public ngOnInit(): void {
    
  }
 
}
