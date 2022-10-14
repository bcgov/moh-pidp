import {
  ChangeDetectionStrategy,
  Component,
  EventEmitter,
  Input,
  Output,
} from '@angular/core';
import { FormGroup } from '@angular/forms';

@Component({
  selector: 'ui-page-v2',
  templateUrl: './page-v2.component.html',
  styleUrls: ['./page-v2.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PageV2Component {
  /**
   * @description
   * Instance of a form.
   */
  @Input() public form?: FormGroup;
  @Input() public autocomplete: 'on' | 'off';
  /**
   * @description
   * Handle submission event emitter.
   */
  @Output() public submitted: EventEmitter<void>;

  public isMobile = false;

  public constructor() {
    this.autocomplete = 'off';
    this.submitted = new EventEmitter<void>();
  }

  public onSubmit(): void {
    this.submitted.emit();
  }
}
