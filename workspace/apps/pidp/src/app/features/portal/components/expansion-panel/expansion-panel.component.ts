import {
  animate,
  state,
  style,
  transition,
  trigger,
} from '@angular/animations';
import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-expansion-panel',
  templateUrl: './expansion-panel.component.html',
  styleUrls: ['./expansion-panel.component.scss'],
  animations: [
    trigger('expandAnimation', [
      state(
        'true',
        style({ height: 'fit-content', opacity: 1, transform: 'translateY(0)' })
      ),
      state(
        'false',
        style({ height: '0', opacity: 0, transform: 'translateY(-10px)' })
      ),
      transition('true <=> false', animate('300ms ease-in')),
    ]),
  ],
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
