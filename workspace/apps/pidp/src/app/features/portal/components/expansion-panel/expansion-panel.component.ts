import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-expansion-panel',
  templateUrl: './expansion-panel.component.html',
  styleUrls: ['./expansion-panel.component.scss'],
})
export class ExpansionPanelComponent {
  @Input() public expanded: boolean | null;
  @Output() public expandedChanged = new EventEmitter<boolean>();

  public title: string;
  public description?: string;
  public backgroundImageSrc?: string;

  public constructor() {
    this.expanded = false;
    this.title = 'Looking to access';
    this.description = 'Provincial Attachment System?';
  }

  public toggle(): void {
    //this.expanded = !this.expanded;
    this.expandedChanged.next(!this.expanded);
  }
}
