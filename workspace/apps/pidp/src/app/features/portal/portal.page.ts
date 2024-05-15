import { STEPPER_GLOBAL_OPTIONS } from '@angular/cdk/stepper';
import { Component, Inject, OnInit } from '@angular/core';

import {
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

  ],
})
export class PortalPage implements OnInit {
 
  public constructor(
    @Inject(APP_CONFIG) private config: AppConfig,
    
  ) {
     
  }

  public ngOnInit(): void {
    
  }
 
}
