import { Component, EventEmitter, Input, Output } from '@angular/core';

import { faUserGroup } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-endorsement-card',
  templateUrl: './endorsement-card.component.html',
  styleUrls: ['./endorsement-card.component.scss'],
})
export class EndorsementCardComponent {
  public faUserGroup = faUserGroup;

  @Input() public id!: number;
  @Input() public nameText!: string;
  @Input() public noticeText: string | null = null;
  @Input() public noticeColour: 'green' | 'yellow' | null = null;
  @Input() public collegeText = '';
  @Input() public createdOnText: string | null = null;
  @Input() public isCancelEnabled = false;
  @Input() public isApproveEnabled = false;

  @Output() public cancelClick = new EventEmitter<number>();
  @Output() public approveClick = new EventEmitter<number>();

  public get showMobileNoticeIcon(): boolean {
    // Show the icon when there is some notice or created on text to display.
    return !!this.noticeText || !!this.createdOnText;
  }
  public get showMobileNoticeText(): boolean {
    // Show then notice text when there is no created on text.
    return !!this.noticeText && !this.createdOnText;
  }
}
