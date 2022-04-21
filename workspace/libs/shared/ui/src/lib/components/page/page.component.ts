import {
  ChangeDetectionStrategy,
  Component,
  EventEmitter,
  Input,
  Output,
} from '@angular/core';
import { FormGroup } from '@angular/forms';

import { Observable, Subscription } from 'rxjs';

import { LoadingService } from '@bcgov/shared/data-access';

@Component({
  selector: 'ui-page',
  templateUrl: './page.component.html',
  styleUrls: ['./page.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PageComponent {
  /**
   * @description
   * Busy subscription for use when blocking content from
   * being interacted with in the template. For example,
   * during but not limited to HTTP requests.
   */
  @Input() public busy?: Subscription;
  /**
   * @description
   * Contraints applied to the maximum width of the
   * view content container to improve reability.
   *
   * "Page" provides readability for a single column of text within
   * a view, and enough room for 2 column forms. While the other sizes
   * provide a bit of flexibility in width when needed for views that
   * are not typical pages with forms found within a specific workflow.
   */
  @Input() public mode: 'page' | 'medium' | 'large' | 'full';
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

  public loading$: Observable<boolean>;

  public constructor(loadingService: LoadingService) {
    this.mode = 'page';
    this.autocomplete = 'off';
    this.submitted = new EventEmitter<void>();
    this.loading$ = loadingService.loading$;
  }

  public onSubmit(): void {
    this.submitted.emit();
  }
}
