import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { AccessRequestResource } from '@app/core/resources/access-request-resource.service';
import { LoggerService } from '@app/core/services/logger.service';

@Component({
  selector: 'app-special-authority-eforms',
  templateUrl: './special-authority-eforms.component.html',
  styleUrls: ['./special-authority-eforms.component.scss'],
})
export class SpecialAuthorityEformsComponent implements OnInit {
  public title: string;

  public constructor(
    private route: ActivatedRoute,
    private accessRequestResource: AccessRequestResource,
    private logger: LoggerService
  ) {
    this.title = this.route.snapshot.data.title;
  }

  public onSubmit(): void {}

  // TODO drop after section are being instantiated
  // public onCardAccessRequestAction(accessType: string): void {
  //   const partyId = this.partyService.profileStatus?.id;

  //   if (!accessType || !partyId) {
  //     return;
  //   }

  //   if (accessType === AccessRequestType.SA_EFORMS) {
  //     // TODO handle error and display notification
  //     this.accessRequestResource.saEforms(partyId).subscribe();
  //   }
  // }

  public ngOnInit(): void {}
}
