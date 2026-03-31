import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';

import { InjectViewportCssClassDirective } from '@bcgov/shared/ui';

@Component({
  selector: 'app-banner',
  templateUrl: './banner.component.html',
  styleUrl: './banner.component.scss',
  standalone: true,
  imports: [CommonModule, InjectViewportCssClassDirective],
})
export class BannerComponent {
  @Input() public banners: Array<{
    header: string;
    body: string;
    status: string;
  }> = [];
}
