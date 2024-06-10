import { HttpErrorResponse } from '@angular/common/http';
import { ErrorHandler, Injectable, Injector } from '@angular/core';
import { Router } from '@angular/router';

import { ErrorService } from './error.service';
import { LoggerService } from './logger.service';

@Injectable({
  providedIn: 'root',
})
export class ErrorHandlerService implements ErrorHandler {
  public constructor(
    // Can't use DI in constructor of the error handler as it's loaded
    // first during bootstrapping therefore have to use the injector
    private injector: Injector,
  ) {}

  public handleError(error: Error | HttpErrorResponse): void {
    const errorService = this.injector.get(ErrorService);
    const logger = this.injector.get(LoggerService);
    const router = this.injector.get(Router);

    const message =
      error instanceof HttpErrorResponse
        ? errorService.getServerMessage(error)
        : errorService.getClientMessage(error);

    const { url } = router;

    logger.error(message, { url }, error);
  }
}
