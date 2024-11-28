import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';

@Component({
  selector: 'app-feedback-button',
  templateUrl: './feedback-button.component.html',
  styleUrl: './feedback-button.component.scss',
  standalone: true,
  imports: [CommonModule],
})
export class FeedbackButtonComponent {

  public constructor() {}
}
