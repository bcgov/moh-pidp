import { ChangeDetectionStrategy, Component } from '@angular/core';

@Component({
  selector: 'ui-page-section',
  templateUrl: './page-section.component.html',
  styleUrls: ['./page-section.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  standalone: true,
})
export class PageSectionComponent {}
