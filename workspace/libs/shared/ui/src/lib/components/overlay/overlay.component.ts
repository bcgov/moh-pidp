import { ChangeDetectionStrategy, Component } from '@angular/core';

import { Observable } from 'rxjs';

import { LoadingOptions, LoadingService } from '@bcgov/shared/data-access';

@Component({
  selector: 'ui-overlay',
  templateUrl: './overlay.component.html',
  styleUrls: ['./overlay.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class OverlayComponent {
  public readonly message: string;
  public readonly loading$: Observable<LoadingOptions | null>;

  public constructor(loadingService: LoadingService) {
    this.message = 'Your request is being processed';
    this.loading$ = loadingService.loading$;
  }
}
