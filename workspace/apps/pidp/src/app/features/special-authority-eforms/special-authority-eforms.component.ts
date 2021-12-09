import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-special-authority-eforms',
  templateUrl: './special-authority-eforms.component.html',
  styleUrls: ['./special-authority-eforms.component.scss'],
})
export class SpecialAuthorityEformsComponent implements OnInit {
  public title: string;

  public constructor(private route: ActivatedRoute) {
    this.title = this.route.snapshot.data.title;
  }

  public ngOnInit(): void {}
}
