import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-college-licence-information',
  templateUrl: './college-licence-information.component.html',
  styleUrls: ['./college-licence-information.component.scss'],
})
export class CollegeLicenceInformationComponent implements OnInit {
  public title: string;
  // TODO temporary variable for demo
  public showPlr: boolean;

  public constructor(private route: ActivatedRoute) {
    this.title = this.route.snapshot.data.title;
    this.showPlr = false;
  }

  // TODO temporary even handler for demo
  public onSelectCollegeLicence(): void {
    this.showPlr = true;
  }

  public ngOnInit(): void {}
}
