import { CommonModule, NgClass } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';

import { FaIconComponent } from '@fortawesome/angular-fontawesome';
import { IconProp } from '@fortawesome/fontawesome-svg-core';
import { faFileLines } from '@fortawesome/free-solid-svg-icons';

import {
  InjectViewportCssClassDirective,
  TextButtonDirective,
} from '@bcgov/shared/ui';

import { ToastService } from '@app/core/services/toast.service';
import { Constants } from '@app/shared/constants';

@Component({
  selector: 'app-access-request-card',
  standalone: true,
  imports: [
    CommonModule,
    FaIconComponent,
    InjectViewportCssClassDirective,
    TextButtonDirective,
    NgClass,
  ],
  templateUrl: './access-request-card.component.html',
  styleUrl: './access-request-card.component.scss',
})
export class AccessRequestCardComponent {
  @Input() public icon: IconProp;
  @Input() public heading: string = '';
  @Input() public description: string = '';
  @Output() public action: EventEmitter<void>;
  @Input() public actionDisabled?: boolean;
  @Input() public completed?: boolean;
  @Input() public errorReason?: string;
  @Input() public completedText!: string;
  public faFileLines = faFileLines;

  public constructor(private readonly toastService: ToastService) {
    this.icon = faFileLines;
    this.action = new EventEmitter<void>();
  }

  public onAction(): void {
    if (this.actionDisabled) {
      this.toastService.openInfoToast(
        (this.errorReason ?? ''),
        Constants.closeText,
        { duration: Constants.dialogDuration, panelClass: 'close-icon' },
      );
    } else {
      this.toastService.closeToast();
      this.action.emit();
    }
  }
}
