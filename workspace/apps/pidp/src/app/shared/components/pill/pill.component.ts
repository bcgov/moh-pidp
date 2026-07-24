import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-pill',
  standalone: true,
  imports: [CommonModule],
  template: `<span class="pill-label">{{ label }}</span>`,
  styleUrls: ['./pill.component.scss'],
})
export class PillComponent {
  @Input() label = '';
}