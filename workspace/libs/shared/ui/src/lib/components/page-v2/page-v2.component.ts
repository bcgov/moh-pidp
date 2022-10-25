import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormGroup } from '@angular/forms';

import { PidpViewport, ViewportService } from '../../services';

@Component({
  selector: 'ui-page-v2',
  templateUrl: './page-v2.component.html',
  styleUrls: ['./page-v2.component.scss'],
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

  public isMobile = true;

  public constructor(viewportService: ViewportService) {
    this.autocomplete = 'off';
    this.submitted = new EventEmitter<void>();
    viewportService.viewportBroadcast$.subscribe((viewport) =>
      this.onViewportChange(viewport)
    );
  }
  private onViewportChange(viewport: PidpViewport): void {
    this.isMobile = viewport === PidpViewport.xsmall;
  }

  public onSubmit(): void {
    this.submitted.emit();
  }
}
