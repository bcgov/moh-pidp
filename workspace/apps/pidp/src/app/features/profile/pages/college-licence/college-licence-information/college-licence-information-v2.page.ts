import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { faStethoscope } from '@fortawesome/free-solid-svg-icons';

import { PartyService } from '@app/core/party/party.service';
import { LoggerService } from '@app/core/services/logger.service';

import { CollegeLicenceInformationResource } from './college-licence-information-resource.service';
import { CollegeLicenceInformationPage } from './college-licence-information.page';

@Component({
  selector: 'app-college-licence-information-v2',
  templateUrl: './college-licence-information-v2.page.html',
  styleUrls: ['./college-licence-information-v2.page.scss'],
})
export class CollegeLicenceInformationV2Page
  extends CollegeLicenceInformationPage
  implements OnInit
{
  public faStethoscope = faStethoscope;

  public constructor(
    resource: CollegeLicenceInformationResource,

    partyService: PartyService,
    router: Router,
    route: ActivatedRoute,
    logger: LoggerService
  ) {
    super(route, router, partyService, resource, logger);
  }
}
