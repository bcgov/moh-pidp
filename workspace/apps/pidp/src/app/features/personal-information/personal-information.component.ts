import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-personal-information',
  templateUrl: './personal-information.component.html',
  styleUrls: ['./personal-information.component.scss'],
})
export class PersonalInformationComponent implements OnInit {
  public title: string;

  public constructor(private route: ActivatedRoute) {
    this.title = this.route.snapshot.data.title;
  }

  public ngOnInit(): void {}
}
