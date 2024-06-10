/* eslint-disable @typescript-eslint/no-explicit-any */
import { HttpErrorResponse } from '@angular/common/http';
import { TestBed } from '@angular/core/testing';

import { of, throwError } from 'rxjs';

import { Spy, provideAutoSpy } from 'jest-auto-spies';

import { NoContentResponse } from '@bcgov/shared/data-access';

import { ApiHttpClient } from '../resources/api-http-client.service';
import {
  ClientLog,
  ClientLogsService,
  MicrosoftLogLevel,
} from './client-logs.service';

describe('ClientLogsService', () => {
  let service: ClientLogsService;
  let apiHttpClientSpy: Spy<ApiHttpClient>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [provideAutoSpy(ApiHttpClient)],
    });

    apiHttpClientSpy = TestBed.inject<any>(ApiHttpClient);

    service = TestBed.inject(ClientLogsService);
  });

  describe('METHOD: createClientLog', () => {
    given('provide a log object', () => {
      const log: ClientLog = {
        message: 'Information message',
        logLevel: MicrosoftLogLevel.INFORMATION,
        additionalInformation: 'UserId',
      };
      apiHttpClientSpy.post.mockReturnValue(of(NoContentResponse));

      when('the method is called', () => {
        service.createClientLog(log);

        then('the log object was sent to the endpoint "client-logs"', () => {
          expect(apiHttpClientSpy.post).toHaveBeenCalledWith(
            'client-logs',
            log,
          );
        });
      });
    });

    given('provide a log object and throw an HttpErrorResponse', (done) => {
      const log: ClientLog = {
        message: 'Information message',
        logLevel: MicrosoftLogLevel.INFORMATION,
        additionalInformation: 'UserId',
      };

      apiHttpClientSpy.post.mockImplementation(() => {
        const err = new HttpErrorResponse({});
        return throwError(() => err);
      });

      when('the method is called', () => {
        then('An exception is thrown', () => {
          service.createClientLog(log).subscribe({
            error: (thrownError) => {
              expect(thrownError).toBeInstanceOf(HttpErrorResponse);
              done();
            },
          });
        });
      });
    });
  });
});
