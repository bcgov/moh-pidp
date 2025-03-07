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
import { DialogBcproviderCreateComponent } from './components/dialog-bcprovider-create.component';
import { DialogBcproviderEditComponent } from './components/dialog-bcprovider-edit.component';

@Component({
    selector: 'app-success-dialog',
    templateUrl: './success-dialog.component.html',
    styleUrl: './success-dialog.component.scss',
    imports: [FaIconComponent, InjectViewportCssClassDirective, NgIf, NgClass]
})
export class SuccessDialogComponent implements OnInit {
  public faCircleCheck = faCircleCheck;
  public faXmark = faXmark;
  public showHeader = false;

  @Input() public username!: string;
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
    if (
      this.componentType instanceof DialogBcproviderCreateComponent ||
      this.componentType instanceof DialogBcproviderEditComponent
    ) {
      this.showHeader = true;
    }
  }

  private loadDialogParagraphComponent(
    componentType: Type<SuccessDialogComponentClass>,
  ): void {
    const componentRef =
      this.dialogParagraphHost.createComponent<SuccessDialogComponentClass>(
        componentType,
      );
    componentRef.instance.username = this.username;
  }
}
