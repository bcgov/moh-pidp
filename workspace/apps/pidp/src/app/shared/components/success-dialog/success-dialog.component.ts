import {
  Component,
  Input,
  OnInit,
  Type,
  ViewChild,
  ViewContainerRef,
} from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { NgIf } from '@angular/common';

import { FaIconComponent } from '@fortawesome/angular-fontawesome';
import { faCircleCheck, faXmark } from '@fortawesome/free-solid-svg-icons';
import { NavigationService } from '@pidp/presentation';

import { InjectViewportCssClassDirective } from '@bcgov/shared/ui';

import { DialogBcproviderCreateComponent } from './components/dialog-bcprovider-create.component';
import { DialogBcproviderEditComponent } from './components/dialog-bcprovider-edit.component';
import { FeedbackSendComponent } from './components/feedback-send.component';

@Component({
  selector: 'app-success-dialog',
  templateUrl: './success-dialog.component.html',
  styleUrl: './success-dialog.component.scss',
  standalone: true,
  imports: [FaIconComponent, InjectViewportCssClassDirective, NgIf],
})
export class SuccessDialogComponent implements OnInit {
  public faCircleCheck = faCircleCheck;
  public faXmark = faXmark;
  public showHeader = false;

  @Input() public username!: string;
  @Input() public title!: string;
  @Input() public componentType!: Type<
    DialogBcproviderCreateComponent | DialogBcproviderEditComponent | FeedbackSendComponent
  >;

  @ViewChild('dialogParagraphHost', { static: true, read: ViewContainerRef })
  public dialogParagraphHost!: ViewContainerRef;

  public constructor(
    public dialog: MatDialog,
    private navigationService: NavigationService,
  ) {}

  public onSuccessDialogClose(): void {
    this.dialog.closeAll();
  }

  public ngOnInit(): void {
    this.loadDialogParagraphComponent(this.componentType);
    if(this.componentType instanceof DialogBcproviderCreateComponent || this.componentType instanceof DialogBcproviderEditComponent) {
      this.showHeader = true;
    }
  }

  private loadDialogParagraphComponent(
    componentType: Type<
      DialogBcproviderCreateComponent | DialogBcproviderEditComponent | FeedbackSendComponent
    >,
  ): void {
    const componentRef = this.dialogParagraphHost.createComponent<
      DialogBcproviderCreateComponent | DialogBcproviderEditComponent | FeedbackSendComponent
    >(componentType);
    componentRef.instance.username = this.username;
  }
}
