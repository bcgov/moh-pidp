import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-site-privacy-security-checklist',
  templateUrl: './site-privacy-security-checklist.page.html',
  styleUrls: ['./site-privacy-security-checklist.page.scss'],
})
export class SitePrivacySecurityChecklistPage implements OnInit {
  public title: string;

  public constructor(private route: ActivatedRoute, private router: Router) {
    this.title = this.route.snapshot.data.title;
  }

  public onSubmit(): void {
    this.navigateToRoot();
  }

  public onBack(): void {
    this.navigateToRoot();
  }

  public ngOnInit(): void {}

  private navigateToRoot(): void {
    this.router.navigate([this.route.snapshot.data.routes.root]);
  }
}
