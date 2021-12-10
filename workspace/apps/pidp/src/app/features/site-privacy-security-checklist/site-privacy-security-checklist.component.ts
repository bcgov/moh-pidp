import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-site-privacy-security-checklist',
  templateUrl: './site-privacy-security-checklist.component.html',
  styleUrls: ['./site-privacy-security-checklist.component.scss'],
})
export class SitePrivacySecurityChecklistComponent implements OnInit {
  public title: string;

  public constructor(private route: ActivatedRoute) {
    this.title = this.route.snapshot.data.title;
  }

  public ngOnInit(): void {}
}
