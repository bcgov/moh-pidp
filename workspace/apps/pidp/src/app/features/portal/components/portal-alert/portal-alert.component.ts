import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';

import { SharedModule } from '../../../../shared/shared.module';

@Component({
  selector: 'app-portal-alert',
  standalone: true,
  template: ` <div>
    <header>
      <h2>{{ heading }}</h2>
    </header>
    <div class="section-content-box">
      <p [innerHtml]="content | safe: 'html'" (click)="onClickAlert()"></p>
    </div>
  </div>`,
  styleUrls: ['./portal-alert.scss'],
  imports: [CommonModule, SharedModule],
})
export class PortalAlertComponent {
  @Input() public heading!: string;
  @Input() public content?: string;
  @Input() public route?: string;
  @Output() public alertClicked = new EventEmitter<string>();

  public onClickAlert(): void {
    if (!this.route) {
      return;
    } else {
      this.alertClicked.emit(this.route);
    }
  }
}
