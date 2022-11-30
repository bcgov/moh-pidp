import { Component, Inject } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { ApplicationService } from '@pidp/presentation';

import { APP_CONFIG, AppConfig } from '@app/app.config';
import { PartyService } from '@app/core/party/party.service';
import { LoggerService } from '@app/core/services/logger.service';

import { DriverFitnessResource } from './driver-fitness-resource.service';
import { DriverFitnessPage } from './driver-fitness.page';

@Component({
  selector: 'app-driver-fitness-v2',
  templateUrl: './driver-fitness-v2.page.html',
  styleUrls: ['./driver-fitness-v2.page.scss'],
})
export class DriverFitnessV2Page extends DriverFitnessPage {
  public constructor(
    @Inject(APP_CONFIG) config: AppConfig,
    route: ActivatedRoute,
    router: Router,
    partyService: PartyService,
    resource: DriverFitnessResource,
    logger: LoggerService,
    applicationService: ApplicationService
  ) {
    super(
      config,
      route,
      router,
      partyService,
      resource,
      logger,
      applicationService
    );
  }
}
