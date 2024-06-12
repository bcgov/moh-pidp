import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-access-request-card',
  standalone: true,
  imports: [],
  templateUrl: './access-request-card.component.html',
  styleUrl: './access-request-card.component.scss',
})
export class AccessRequestCardComponent {
  @Input() public icon: string = '';
  @Input() public heading: string = '';
  @Input() public description: string = '';
}
