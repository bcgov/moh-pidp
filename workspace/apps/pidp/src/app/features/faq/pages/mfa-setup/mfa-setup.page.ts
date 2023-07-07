import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { UtilsService } from '@app/core/services/utils.service';

@Component({
  selector: 'app-mfa-setup',
  templateUrl: './mfa-setup.page.html',
  styleUrls: ['./mfa-setup.page.scss'],
})
export class MfaSetupPage implements OnInit {
  public constructor(
    private route: ActivatedRoute,
    private router: Router,
    private utilsService: UtilsService
  ) {}

  public onBack(): void {
    this.navigateToRoot();
  }

  public ngOnInit(): void {
    this.utilsService.scrollTop();
  }

  private navigateToRoot(): void {
    this.router.navigate([this.route.snapshot.data.routes.root]);
  }
}
