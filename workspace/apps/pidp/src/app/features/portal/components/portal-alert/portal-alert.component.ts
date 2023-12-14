import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { SharedModule } from "../../../../shared/shared.module";

@Component({
    selector: 'app-portal-alert',
    standalone: true,
    template: `
    <div>
      <header>
        <h2>{{ heading }}</h2>
      </header>
      <div class="section-content-box">
        <p [innerHtml]="content | safe: 'html'"></p>
      </div>
      <!-- Work around for ngProjectAs not passing directive reference when applied to ng-content -->
      <ng-container *ngIf="actionTemplate.length">
        <ng-content select="[appPortalAlert]"></ng-content>
      </ng-container>
    </div>`,
    styles: [],
    imports: [CommonModule, SharedModule]
})
export class PortalAlertComponent {
  @Input() public heading!: string;
  @Input() public content?: string;
  @Input() public actionTemplate?:
}
