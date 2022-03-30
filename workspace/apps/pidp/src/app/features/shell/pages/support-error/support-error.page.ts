import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { ShellRoutes } from '../../shell.routes';

@Component({
  selector: 'app-support-error',
  templateUrl: './support-error.page.html',
  styleUrls: ['./support-error.page.scss'],
})
export class SupportErrorPage implements OnInit {
  public constructor(private route: ActivatedRoute, private router: Router) {}

  public onBack(): void {
    this.navigateToRoot();
  }

  public ngOnInit(): void {}

  private navigateToRoot(): void {
    this.router.navigate([ShellRoutes.MODULE_PATH]);
  }
}
