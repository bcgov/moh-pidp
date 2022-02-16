import {
  ChangeDetectionStrategy,
  Component,
  ComponentFactoryResolver,
  Inject,
  OnInit,
  ViewChild,
  ViewContainerRef,
} from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

import { IDialogContent } from '../dialog-content.model';
import { DialogDefaultOptions } from '../dialog-default-options.model';
import { DialogOptions } from '../dialog-options.model';
import { DialogContentOutput } from '../dialog-output.model';
import { DIALOG_DEFAULT_OPTION } from '../dialogs-properties.provider';

// TODO use generics to get typings in place and drop use of any
@Component({
  selector: 'ui-confirm-dialog',
  templateUrl: './confirm-dialog.component.html',
  styleUrls: ['./confirm-dialog.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ConfirmDialogComponent implements OnInit {
  public options: DialogOptions;
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  public dialogContentOutput: DialogContentOutput<any> | null;

  @ViewChild('dialogContentHost', { static: true, read: ViewContainerRef })
  public dialogContentHost!: ViewContainerRef;

  public constructor(
    public dialogRef: MatDialogRef<ConfirmDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public customOptions: DialogOptions,
    @Inject(DIALOG_DEFAULT_OPTION) public defaultOptions: DialogDefaultOptions,
    private componentFactoryResolver: ComponentFactoryResolver
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
        this.options.data
      );
    }
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

  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  private loadDialogContentComponent(component: any, data: any): void {
    const componentFactory =
      this.componentFactoryResolver.resolveComponentFactory(component);
    this.dialogContentHost.clear();

    // TODO dynamic component creation in v13 has an issue with rendering the generate component vs the deprecated API
    const componentRef =
      this.dialogContentHost.createComponent(componentFactory);
    const componentInstance = componentRef.instance as IDialogContent;
    componentInstance.data = data;
    const output$ = componentInstance.output;

    if (output$) {
      // eslint-disable-next-line @typescript-eslint/no-explicit-any
      output$.subscribe((value: any) => (this.dialogContentOutput = value));
    }
  }
}
