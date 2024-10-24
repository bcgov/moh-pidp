import { NgIf } from '@angular/common';
import {
  AfterViewInit,
  ChangeDetectionStrategy,
  Component,
  Inject,
  OnInit,
  Type,
  ViewChild,
  ViewContainerRef,
} from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import {
  MAT_DIALOG_DATA,
  MatDialogActions,
  MatDialogClose,
  MatDialogContent,
  MatDialogRef,
  MatDialogTitle,
} from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';

import { AnchorDirective } from '../../anchor/anchor.directive';
import { IDialogContent } from '../dialog-content.model';
import { DialogDefaultOptions } from '../dialog-default-options.model';
import { DialogOptions } from '../dialog-options.model';
import { DialogContentOutput } from '../dialog-output.model';
import { DIALOG_DEFAULT_OPTION } from '../dialogs-properties.provider';

@Component({
  selector: 'ui-confirm-dialog',
  templateUrl: './confirm-dialog.component.html',
  styleUrls: ['./confirm-dialog.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  standalone: true,
  imports: [
    AnchorDirective,
    MatDialogTitle,
    NgIf,
    MatIconModule,
    MatDialogContent,
    MatDialogActions,
    MatButtonModule,
    MatDialogClose,
    AnchorDirective,
  ],
})
export class ConfirmDialogComponent implements OnInit, AfterViewInit {
  public options: DialogOptions;
  public dialogContentOutput: DialogContentOutput<unknown> | null;

  @ViewChild('dialogContentHost', { static: true, read: ViewContainerRef })
  public dialogContentHost!: ViewContainerRef;

  public constructor(
    public dialogRef: MatDialogRef<ConfirmDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public customOptions: DialogOptions,
    @Inject(DIALOG_DEFAULT_OPTION) public defaultOptions: DialogDefaultOptions,
  ) {
    this.options =
      typeof customOptions === 'string'
        ? this.getOptions(defaultOptions[customOptions]())
        : this.getOptions(customOptions);

    this.dialogContentOutput = null;
  }

  public onConfirm(): void {
    const response =
      this.dialogContentOutput !== null
        ? { output: this.dialogContentOutput }
        : true;
    this.dialogRef.close(response);
  }

  public ngOnInit(): void {
    if (this.options.component) {
      this.loadDialogContentComponent(
        this.options.component,
        this.options.data,
      );
    }

    this.dialogRef.updateSize(this.options.width, this.options.height);
    this.options.class && this.dialogRef.addPanelClass(this.options.class);
  }

  public ngAfterViewInit(): void {
    const adjustSubmitFocus = document.querySelector(
      '#submit',
    ) as HTMLButtonElement;
    if (adjustSubmitFocus) adjustSubmitFocus.focus();
  }

  private getOptions(dialogOptions: DialogOptions): DialogOptions {
    const options: DialogOptions = {
      actionType: 'primary',
      actionText: 'Confirm',
      cancelText: 'Cancel',
      cancelHide: false,
      ...dialogOptions,
    };

    return {
      icon: options.actionType === 'warn' ? 'warning' : 'help',
      ...options,
    };
  }

  private loadDialogContentComponent(
    component: Type<unknown>,
    data: unknown,
  ): void {
    this.dialogContentHost.clear();

    const componentRef = this.dialogContentHost.createComponent(component);
    const componentInstance = componentRef.instance as IDialogContent;
    componentInstance.data = data;
    const output$ = componentInstance.output;

    if (output$) {
      output$.subscribe(
        (value: DialogContentOutput<unknown> | null) =>
          (this.dialogContentOutput = value),
      );
    }
  }
}
