import { Component, Input } from '@angular/core';

import { PidpViewport, ViewportService } from '@bcgov/shared/ui';

@Component({
  selector: 'app-auth-system-card',
  templateUrl: './system-card.page.html',
  styleUrls: ['./system-card.page.scss'],
})
export class SystemCardComponent {
  @Input() public titleText = '';
  @Input() public imageUrls = [''];
  @Input() public imageTitleText = '';
  @Input() public headerColor: 'white' | 'grey' = 'white';

  public viewportOptions = PidpViewport;
  public viewport = PidpViewport.xsmall;

  public constructor(private viewportService: ViewportService) {
    this.viewportService.viewportBroadcast$.subscribe((viewport) =>
      this.onViewportChange(viewport)
    );
  }
  private onViewportChange(viewport: PidpViewport): void {
    this.viewport = viewport;
  }
  public getImageUrl(): string {
    let imageUrl: string;
    switch (this.viewport) {
      case PidpViewport.xsmall:
        imageUrl = this.imageUrls[0];
        break;
      case PidpViewport.small:
      case PidpViewport.medium:
      case PidpViewport.large:
        imageUrl = this.imageUrls[1];
        break;
      default:
        throw 'not implemented: ' + this.viewport;
    }
    return imageUrl;
  }
}
