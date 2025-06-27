import { AnimationEvent } from '@angular/animations';
import { NgClass } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';

import { Subject, distinctUntilChanged } from 'rxjs';

import {
  ExpansionPanelState,
  appExpansionAnimations,
} from './expansion-animation';

@Component({
    selector: 'ui-expansion-panel',
    templateUrl: './expansion-panel.component.html',
    styleUrls: ['./expansion-panel.component.scss'],
    animations: [appExpansionAnimations.bodyExpansion],
    imports: [NgClass]
})
export class ExpansionPanelComponent {
  private _expanded: boolean;

  @Output() public expandedChanged = new EventEmitter<boolean>();

  /** Stream of body animation done events. */
  public readonly _bodyAnimationDone = new Subject<AnimationEvent>();

  public get expanded(): boolean {
    return this._expanded;
  }

  @Input() public set expanded(value: boolean) {
    this._expanded = value;
  }

  public constructor() {
    this._expanded = false;

    // We need a Subject with distinctUntilChanged, because the `done` event
    // fires twice on some browsers. See https://github.com/angular/angular/issues/24084
    this._bodyAnimationDone
      .pipe(
        distinctUntilChanged((x, y) => {
          return x.fromState === y.fromState && x.toState === y.toState;
        }),
      )
      .subscribe((event: AnimationEvent) => {
        if (event.fromState !== 'void') {
          if (event.toState === 'expanded' && this._expanded) {
            this.expandedChanged.next(true);
          } else if (event.toState === 'collapsed' && !this._expanded) {
            this.expandedChanged.next(false);
          }
        }
      });
  }

  public toggle(): void {
    this._expanded = !this.expanded;
  }

  /** Gets the expanded state string. */
  public _getExpandedState(): ExpansionPanelState {
    return this._expanded ? 'expanded' : 'collapsed';
  }
}
