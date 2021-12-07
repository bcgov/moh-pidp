import { Component, ChangeDetectionStrategy, Input } from '@angular/core';

@Component({
  selector: 'ui-page-section',
  templateUrl: './page-section.component.html',
  styleUrls: ['./page-section.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PageSectionComponent {
  @Input() public class: string;

  public constructor() {
    this.class = 'mb-4';
  }
}
