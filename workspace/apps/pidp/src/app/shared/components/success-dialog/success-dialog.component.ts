import {
  Component,
  Input,
  OnInit,
  Type,
  ViewChild,
  ViewContainerRef,
} from '@angular/core';
import { MatDialog } from '@angular/material/dialog';

import { faCircleCheck, faXmark } from '@fortawesome/free-solid-svg-icons';
import { NavigationService } from '@pidp/presentation';

@Component({
  selector: 'app-success-dialog',
  templateUrl: './success-dialog.component.html',
  styleUrl: './success-dialog.component.scss',
})
export class SuccessDialogComponent implements OnInit {
  public faCircleCheck = faCircleCheck;
  public faXmark = faXmark;

  @Input() public username!: string;
  @Input() public title!: string;
  @Input() public componentType!: Type<unknown>;

  @ViewChild('dialogParagraphHost', { static: true, read: ViewContainerRef })
  public dialogParagraphHost!: ViewContainerRef;

  public constructor(
    public dialog: MatDialog,
    private navigationService: NavigationService,
  ) {}

  public onSuccessDialogClose(): void {
    this.dialog.closeAll();
    this.navigationService.navigateToRoot();
  }

  public ngOnInit(): void {
    this.loadDialogParagraphComponent(this.componentType);
  }

  private loadDialogParagraphComponent(componentType: Type<unknown>): void {
    const componentRef =
      this.dialogParagraphHost.createComponent<unknown>(componentType);
    componentRef.instance.username = this.username;
  }
}
