import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-terms-of-access-agreement',
  templateUrl: './terms-of-access-agreement.component.html',
  styleUrls: ['./terms-of-access-agreement.component.scss'],
})
export class TermsOfAccessAgreementComponent implements OnInit {
  public title: string;

  public constructor(private route: ActivatedRoute) {
    this.title = this.route.snapshot.data.title;
  }

  public ngOnInit(): void {}
}
