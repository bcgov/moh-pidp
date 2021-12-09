import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-work-and-role-information',
  templateUrl: './work-and-role-information.component.html',
  styleUrls: ['./work-and-role-information.component.scss'],
})
export class WorkAndRoleInformationComponent implements OnInit {
  public title: string;

  public constructor(private route: ActivatedRoute) {
    this.title = this.route.snapshot.data.title;
  }

  public ngOnInit(): void {}
}
