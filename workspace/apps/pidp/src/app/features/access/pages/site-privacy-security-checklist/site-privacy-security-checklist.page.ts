import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { DemoService } from '@core/services/demo.service';

@Component({
  selector: 'app-site-privacy-security-checklist',
  templateUrl: './site-privacy-security-checklist.page.html',
  styleUrls: ['./site-privacy-security-checklist.page.scss'],
})
export class SitePrivacySecurityChecklistPage implements OnInit {
  public title: string;

  public constructor(
    private route: ActivatedRoute,
    private demoService: DemoService
  ) {
    this.title = this.route.snapshot.data.title;
  }

  public onSubmit(): void {
    this.demoService.state.accessToSystemsSections =
      this.demoService.state.accessToSystemsSections.map((section) => {
        // if (section.type === 'site-privacy-and-security-readiness-checklist') {
        //   return {
        //     ...section,
        //     statusType: 'success',
        //     status: 'completed',
        //   };
        // }
        return section;
      });
  }

  public ngOnInit(): void {}
}
