import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { DemoService } from '@core/services/demo.service';

@Component({
  selector: 'app-college-licence-information',
  templateUrl: './college-licence-information.component.html',
  styleUrls: ['./college-licence-information.component.scss'],
})
export class CollegeLicenceInformationComponent implements OnInit {
  public title: string;
  // TODO temporary variable for demo
  public showPlr: boolean;

  public constructor(
    private route: ActivatedRoute,
    private demoService: DemoService
  ) {
    this.title = this.route.snapshot.data.title;
    this.showPlr = false;
  }

  public onSubmit(): void {
    this.demoService.state.profileIdentitySections =
      this.demoService.state.profileIdentitySections.map((section) => {
        if (section.type === 'college-licence-information') {
          return {
            ...section,
            statusType: 'success',
            status: 'completed',
          };
        }
        if (section.type === 'work-and-role-information') {
          return {
            ...section,
            disabled: false,
          };
        }
        return section;
      });
  }

  // TODO temporary even handler for demo
  public onSelectCollegeLicence(): void {
    this.showPlr = true;
  }

  public ngOnInit(): void {}
}
