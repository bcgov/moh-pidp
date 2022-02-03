import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { DemoService } from '@core/services/demo.service';

@Component({
  selector: 'app-next-steps',
  templateUrl: './next-steps.page.html',
  styleUrls: ['./next-steps.page.scss'],
})
export class NextStepsPage implements OnInit {
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
        // if (section.type === 'pharmanet') {
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
