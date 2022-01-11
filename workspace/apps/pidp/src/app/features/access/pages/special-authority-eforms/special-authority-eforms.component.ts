import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { DemoService } from '@core/services/demo.service';

@Component({
  selector: 'app-special-authority-eforms',
  templateUrl: './special-authority-eforms.component.html',
  styleUrls: ['./special-authority-eforms.component.scss'],
})
export class SpecialAuthorityEformsComponent implements OnInit {
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
        if (section.type === 'special-authority-eforms') {
          return {
            ...section,
            statusType: 'success',
            status: 'completed',
          };
        }
        return section;
      });
  }

  public ngOnInit(): void {}
}
