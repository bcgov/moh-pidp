import { NgClass, NgIf } from '@angular/common';
import {
  Component,
  Input,
  OnInit,
  Type,
  ViewChild,
  ViewContainerRef,
} from '@angular/core';
import { MatDialog } from '@angular/material/dialog';

import { FaIconComponent } from '@fortawesome/angular-fontawesome';
import { faCircleCheck, faXmark } from '@fortawesome/free-solid-svg-icons';

import { InjectViewportCssClassDirective } from '@bcgov/shared/ui';

import { SuccessDialogComponentClass } from './classes/success-dialog-component.class';
import { FeedbackSendComponent } from './components/feedback-send.component';

@Component({
  selector: 'app-success-dialog',
  templateUrl: './success-dialog.component.html',
  styleUrl: './success-dialog.component.scss',
  standalone: true,
  imports: [FaIconComponent, InjectViewportCssClassDirective, NgIf, NgClass],
})
export class SuccessDialogComponent implements OnInit {
  public faCircleCheck = faCircleCheck;
  public faXmark = faXmark;
  public showHeader = false;

  @Input() public username = '';
  @Input() public title!: string;
  @Input() public componentType!: Type<SuccessDialogComponentClass>;

  @ViewChild('dialogParagraphHost', { static: true, read: ViewContainerRef })
  public dialogParagraphHost!: ViewContainerRef;

  public constructor(public dialog: MatDialog) {}

  public onSuccessDialogClose(): void {
    this.dialog.closeAll();
  }

  public ngOnInit(): void {
    this.loadDialogParagraphComponent(this.componentType);
    this.showHeader = this.componentType !== FeedbackSendComponent;
  }

  private loadDialogParagraphComponent(
    componentType: Type<SuccessDialogComponentClass>,
  ): void {
    if (!this.dialogParagraphHost) {
      console.error('DialogParagraphHost is not initialized.');
      return;
    }
    const componentRef =
      this.dialogParagraphHost.createComponent<SuccessDialogComponentClass>(
        componentType,
      );

    if ('username' in componentRef.instance) {
      componentRef.instance.username = this.username;
    }
  }
}
