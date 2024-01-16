import {
  ChangeDetectionStrategy,
  Component,
  ContentChildren,
  EventEmitter,
  Input,
  Output,
  QueryList,
} from '@angular/core';
import { ThemePalette } from '@angular/material/core';
import { MenuPositionX, MenuPositionY, MatMenuModule } from '@angular/material/menu';

import { ContextHelpContentDirective } from '../context-help-content.directive';
import { ContextHelpTitleDirective } from '../context-help-title.directive';
import { NgIf } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';

@Component({
    selector: 'ui-context-help',
    templateUrl: './context-help.component.html',
    styleUrls: ['./context-help.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
    standalone: true,
    imports: [
        MatButtonModule,
        MatMenuModule,
        MatIconModule,
        NgIf,
    ],
})
export class ContextHelpComponent {
  @Input() public xPosition: MenuPositionX;
  @Input() public yPosition: MenuPositionY;
  @Input() public icon: string;
  @Input() public menuIconColor: ThemePalette;
  @Input() public small: boolean;
  @Input() public outlined: boolean;
  @Input() public advanced: boolean;
  @Input() public titleIcon?: string;
  @Output() public opened: EventEmitter<void>;

  @ContentChildren(ContextHelpTitleDirective, { descendants: true })
  public contextHelpTitleChildren?: QueryList<ContextHelpTitleDirective>;
  @ContentChildren(ContextHelpContentDirective, { descendants: true })
  public contextHelpContentChildren?: QueryList<ContextHelpContentDirective>;

  public constructor() {
    this.xPosition = 'after';
    this.yPosition = 'below';
    this.icon = 'info';
    this.menuIconColor = 'primary';
    this.small = false;
    this.outlined = false;
    this.advanced = false;
    this.opened = new EventEmitter<void>();
  }

  public onOpenContextMenu(event: Event): void {
    event.stopPropagation();
    this.opened.emit();
  }
}
