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
import { NavigationService } from '@pidp/presentation';

import { InjectViewportCssClassDirective } from '@bcgov/shared/ui';

import { DialogBcproviderCreateComponent } from './components/dialog-bcprovider-create.component';
import { DialogBcproviderEditComponent } from './components/dialog-bcprovider-edit.component';

@Component({
  selector: 'app-success-dialog',
  templateUrl: './success-dialog.component.html',
  styleUrl: './success-dialog.component.scss',
  standalone: true,
  imports: [FaIconComponent, InjectViewportCssClassDirective],
})
export class SuccessDialogComponent implements OnInit {
  public faCircleCheck = faCircleCheck;
  public faXmark = faXmark;

  @Input() public username!: string;
  @Input() public title!: string;
  @Input() public componentType!: Type<
    DialogBcproviderCreateComponent | DialogBcproviderEditComponent
  >;

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

  private loadDialogParagraphComponent(
    componentType: Type<
      DialogBcproviderCreateComponent | DialogBcproviderEditComponent
    >,
  ): void {
    const componentRef = this.dialogParagraphHost.createComponent<
      DialogBcproviderCreateComponent | DialogBcproviderEditComponent
    >(componentType);
    componentRef.instance.username = this.username;
  }
}
