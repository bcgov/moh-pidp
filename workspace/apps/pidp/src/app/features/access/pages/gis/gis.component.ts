import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { DemoService } from '@core/services/demo.service';

@Component({
  selector: 'app-gis',
  templateUrl: './gis.component.html',
  styleUrls: ['./gis.component.scss'],
})
export class GisComponent implements OnInit {
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
        if (section.type === 'gis') {
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
