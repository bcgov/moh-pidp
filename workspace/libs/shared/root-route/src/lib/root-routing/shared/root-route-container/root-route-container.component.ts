import { NgOptimizedImage } from '@angular/common';
import { Component } from '@angular/core';

@Component({
  selector: 'ui-root-route-container',
  templateUrl: './root-route-container.component.html',
  styleUrls: ['./root-route-container.component.scss'],
  standalone: true,
  imports: [NgOptimizedImage],
})
export class RootRouteContainerComponent {}
