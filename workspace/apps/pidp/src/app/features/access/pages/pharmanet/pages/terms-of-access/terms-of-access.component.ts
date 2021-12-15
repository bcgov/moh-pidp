import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-terms-of-access',
  templateUrl: './terms-of-access.component.html',
  styleUrls: ['./terms-of-access.component.scss'],
})
export class TermsOfAccessComponent implements OnInit {
  public title: string;

  public constructor(private route: ActivatedRoute) {
    this.title = this.route.snapshot.data.title;
  }

  public ngOnInit(): void {}
}
