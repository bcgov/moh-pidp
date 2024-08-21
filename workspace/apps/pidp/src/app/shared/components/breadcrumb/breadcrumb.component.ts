import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';

import { FaIconComponent } from '@fortawesome/angular-fontawesome';
import { faAngleRight } from '@fortawesome/free-solid-svg-icons';
import { NavigationService } from '@pidp/presentation';

import {
  InjectViewportCssClassDirective,
  TextButtonDirective,
} from '@bcgov/shared/ui';

import { AccessRoutes } from '@app/features/access/access.routes';

@Component({
  selector: 'app-breadcrumb',
  templateUrl: './breadcrumb.component.html',
  styleUrl: './breadcrumb.component.scss',
  standalone: true,
  imports: [
    CommonModule,
    InjectViewportCssClassDirective,
    FaIconComponent,
    TextButtonDirective,
  ],
})
export class BreadcrumbComponent {
  public faAngleRight = faAngleRight;
  public AccessRoutes = AccessRoutes;
  @Input() public breadcrumbs: Array<{ title: string; path: string }> = [];

  public constructor(
    private router: Router,
    private navigationService: NavigationService,
  ) {}

  public onBack(): void {
    this.navigationService.navigateToRoot();
  }

  public navigateTo(path: string): void {
    this.router.navigateByUrl(path);
  }
}
