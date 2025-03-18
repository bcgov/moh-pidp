import { CommonModule, NgOptimizedImage } from '@angular/common';
import { Component } from '@angular/core';
import { BreadcrumbComponent } from '@app/shared/components/breadcrumb/breadcrumb.component';
import { InjectViewportCssClassDirective } from '@bcgov/shared/ui';
import { AccessRoutes } from '../../access.routes';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { RouterLink } from '@angular/router';

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

}
