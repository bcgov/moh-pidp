import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { DemoService } from '@core/services/demo.service';

@Component({
  selector: 'app-terms-of-access-agreement',
  templateUrl: './terms-of-access-agreement.component.html',
  styleUrls: ['./terms-of-access-agreement.component.scss'],
})
export class TermsOfAccessAgreementComponent implements OnInit {
  public title: string;

  public constructor(
    private route: ActivatedRoute,
    private demoService: DemoService
  ) {
    this.title = this.route.snapshot.data.title;
  }

  public onSubmit(): void {
    this.demoService.profileComplete = true;

    this.demoService.state.profileIdentitySections =
      this.demoService.state.profileIdentitySections.map((section) => {
        if (section.type === 'terms-of-access-agreement') {
          return {
            ...section,
            statusType: 'success',
            status: 'completed',
          };
        }

        return section;
      });

    this.demoService.state.accessToSystemsSections =
      this.demoService.state.accessToSystemsSections.map((section) => {
        section.disabled = false;
        return section;
      });

    this.demoService.state.trainingSections =
      this.demoService.state.trainingSections.map((section) => {
        section.disabled = false;
        return section;
      });

    this.demoService.state.yourProfileSections =
      this.demoService.state.yourProfileSections.map((section) => {
        section.disabled = false;
        return section;
      });
  }

  public ngOnInit(): void {}
}
