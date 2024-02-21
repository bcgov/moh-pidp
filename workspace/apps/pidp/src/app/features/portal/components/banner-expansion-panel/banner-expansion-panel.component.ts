import {
  AnimationEvent,
  animate,
  state,
  style,
  transition,
  trigger,
} from '@angular/animations';
import { NgTemplateOutlet } from '@angular/common';
import {
  Component,
  EventEmitter,
  Input,
  OnDestroy,
  Output,
} from '@angular/core';
import { MatButtonModule } from '@angular/material/button';

import { Subject, distinctUntilChanged } from 'rxjs';

import { InjectViewportCssClassDirective } from '@bcgov/shared/ui';

export const EXPANSION_PANEL_ANIMATION_TIMING =
  '500ms cubic-bezier(0.4,0.0,0.2,1)';

/** ExpansionPanel's states. */
export type ExpansionPanelState = 'expanded' | 'collapsed';

@Component({
  selector: 'app-banner-expansion-panel',
  templateUrl: './banner-expansion-panel.component.html',
  styleUrls: ['./banner-expansion-panel.component.scss'],
  animations: [
    trigger('bodyExpansion', [
      state('collapsed, void', style({ height: '0', visibility: 'hidden' })),
      state('expanded', style({ height: '*', visibility: '' })),
      transition(
        'expanded <=> collapsed, void => collapsed',
        animate(EXPANSION_PANEL_ANIMATION_TIMING),
      ),
    ]),
  ],
  standalone: true,
  imports: [InjectViewportCssClassDirective, MatButtonModule, NgTemplateOutlet],
})
export class BannerExpansionPanelComponent implements OnDestroy {
  private _expanded: boolean;
  @Output() public expandedChanged = new EventEmitter<boolean>();

  public title: string;
  public description?: string;
  public backgroundImageSrc?: string;

  /** Stream of body animation done events. */
  public readonly _bodyAnimationDone = new Subject<AnimationEvent>();

  public get expanded(): boolean {
    return this._expanded;
  }
  @Input() public set expanded(value: boolean | null) {
    this._expanded = value ?? false;
  }

  public constructor() {
    this._expanded = false;
    this.title = 'Start here to access';
    this.description = 'Provincial Attachment System';

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
    this._expanded = !this._expanded;
  }

  /** Gets the expanded state string. */
  public _getExpandedState(): ExpansionPanelState {
    return this._expanded ? 'expanded' : 'collapsed';
  }

  public ngOnDestroy(): void {
    this._bodyAnimationDone.complete();
  }
}
