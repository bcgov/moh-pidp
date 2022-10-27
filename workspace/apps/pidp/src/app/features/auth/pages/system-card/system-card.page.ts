import { Component, Input } from '@angular/core';

import { PidpViewport, ViewportService } from '@bcgov/shared/ui';

@Component({
  selector: 'app-auth-system-card',
  templateUrl: './system-card.page.html',
  styleUrls: ['./system-card.page.scss'],
})
export class SystemCardComponent {
  @Input() public titleText = '';
  @Input() public webpImageUrls = [''];
  @Input() public jpegImageUrls = [''];
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
  public getImageUrl(format: 'webp' | 'jpeg'): string {
    let imageUrl: string;
    switch (this.viewport) {
      case PidpViewport.xsmall:
        imageUrl = this.getImageUrlFromFormat(0, format);
        break;
      case PidpViewport.small:
        imageUrl = this.getImageUrlFromFormat(1, format);
        break;
      case PidpViewport.medium:
        imageUrl = this.getImageUrlFromFormat(2, format);
        break;
      case PidpViewport.large:
        imageUrl = this.getImageUrlFromFormat(3, format);
        break;
      default:
        throw 'getImageUrl: not implemented ' + this.viewport;
    }
    return imageUrl;
  }
  private getImageUrlFromFormat(
    index: number,
    format: 'webp' | 'jpeg'
  ): string {
    switch (format) {
      case 'webp':
        return this.webpImageUrls[index];
      case 'jpeg':
        return this.jpegImageUrls[index];
      default:
        throw 'getImageUrlFromFormat: not implemented ' + format;
    }
  }
}
