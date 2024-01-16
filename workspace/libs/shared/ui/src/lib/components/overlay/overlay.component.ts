import { ChangeDetectionStrategy, Component } from '@angular/core';

import { Observable } from 'rxjs';

import { LoadingOptions, LoadingService } from '@bcgov/shared/data-access';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { NgIf, AsyncPipe } from '@angular/common';

@Component({
    selector: 'ui-overlay',
    templateUrl: './overlay.component.html',
    styleUrls: ['./overlay.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
    standalone: true,
    imports: [
        NgIf,
        MatProgressSpinnerModule,
        AsyncPipe,
    ],
})
export class OverlayComponent {
  public readonly message: string;
  public readonly loading$: Observable<LoadingOptions | null>;

  public constructor(loadingService: LoadingService) {
    this.message = 'Your request is being processed';
    this.loading$ = loadingService.loading$;
  }
}
