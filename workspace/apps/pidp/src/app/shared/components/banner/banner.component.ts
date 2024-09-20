import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-banner',
  templateUrl: './banner.component.html',
  styleUrl: './banner.component.scss',
  standalone: true,
  imports: [
    CommonModule
  ],
})
export class BannerComponent {
  @Input() public banners: Array<{ header: string; body: string; status: string; }> = [];

  public constructor(
  ) {}
}
