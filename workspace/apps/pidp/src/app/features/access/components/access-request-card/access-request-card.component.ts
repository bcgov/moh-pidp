import { Component, Input } from '@angular/core';

import { FaIconComponent } from '@fortawesome/angular-fontawesome';
import { IconProp } from '@fortawesome/fontawesome-svg-core';
import { faFileLines } from '@fortawesome/free-solid-svg-icons';

import { InjectViewportCssClassDirective } from '@bcgov/shared/ui';

@Component({
  selector: 'app-access-request-card',
  standalone: true,
  imports: [FaIconComponent, InjectViewportCssClassDirective],
  templateUrl: './access-request-card.component.html',
  styleUrl: './access-request-card.component.scss',
})
export class AccessRequestCardComponent {
  @Input() public icon: IconProp;
  @Input() public heading: string = '';
  @Input() public description: string = '';
  public faFileLines = faFileLines;
  public constructor() {
    this.icon = faFileLines;
  }
}
