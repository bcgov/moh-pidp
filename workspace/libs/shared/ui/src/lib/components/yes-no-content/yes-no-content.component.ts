import { NgFor, NgIf } from '@angular/common';
import {
  ChangeDetectionStrategy,
  Component,
  EventEmitter,
  Output,
} from '@angular/core';
import { MatRadioChange, MatRadioModule } from '@angular/material/radio';

@Component({
    selector: 'ui-yes-no-content',
    templateUrl: './yes-no-content.component.html',
    styleUrls: ['./yes-no-content.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
    imports: [MatRadioModule, NgFor, NgIf]
})
export class YesNoContentComponent {
  @Output() public update: EventEmitter<string>;

  public decisions: { code: boolean; name: string }[];
  public chosenDecision: string | null;

  public constructor() {
    this.decisions = [
      { code: false, name: 'No' },
      { code: true, name: 'Yes' },
    ];
    this.chosenDecision = null;
    this.update = new EventEmitter<string>();
  }

  public onDecisionUpdate({ value }: MatRadioChange): void {
    this.chosenDecision = value;
  }
}
