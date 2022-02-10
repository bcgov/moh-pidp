import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-terms-of-access',
  templateUrl: './terms-of-access.page.html',
  styleUrls: ['./terms-of-access.page.scss'],
})
export class TermsOfAccessPage implements OnInit {
  public title: string;

  public constructor(private route: ActivatedRoute) {
    this.title = this.route.snapshot.data.title;
  }

  public ngOnInit(): void {}
}
