import { CommonModule, NgOptimizedImage } from '@angular/common';
import { Component } from '@angular/core';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { Router, RouterLink } from '@angular/router';

import { InjectViewportCssClassDirective } from '@bcgov/shared/ui';

import { BreadcrumbComponent } from '@app/shared/components/breadcrumb/breadcrumb.component';

import { AccessRoutes } from '../../access.routes';

@Component({
  selector: 'app-b2b-information-page',
  standalone: true,
  imports: [
    BreadcrumbComponent,
    CommonModule,
    InjectViewportCssClassDirective,
    MatFormFieldModule,
    MatInputModule,
    NgOptimizedImage,
    RouterLink,
  ],
  templateUrl: './b2b-information-page.component.html',
  styleUrl: './b2b-information-page.component.scss',
})
export class B2bInformationPageComponent {
  public AccessRoutes = AccessRoutes;
  public breadcrumbsData: Array<{ title: string; path: string }> = [
    { title: 'Home', path: '' },
    {
      title: 'Access',
      path: AccessRoutes.routePath(AccessRoutes.ACCESS_REQUESTS),
    },
    { title: 'B2B Information', path: '' },
  ];

  public constructor(private router: Router) {}

  public onNext(): void {
    this.router.navigate([AccessRoutes.routePath(AccessRoutes.IMMSBC)]);
  }
}
