import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { DemoService } from '@core/services/demo.service';

@Component({
  selector: 'app-compliance-training',
  templateUrl: './compliance-training.page.html',
  styleUrls: ['./compliance-training.page.scss'],
})
export class ComplianceTrainingPage implements OnInit {
  public title: string;

  public constructor(
    private route: ActivatedRoute,
    private demoService: DemoService
  ) {
    this.title = this.route.snapshot.data.title;
  }

  public onSubmit(): void {
    this.demoService.state.trainingSections =
      this.demoService.state.trainingSections.map((section) => {
        // if (section.type === 'compliance-training') {
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
