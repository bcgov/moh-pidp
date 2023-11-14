import { TestBed } from '@angular/core/testing';

import { ClientLogsService } from './client-logs.service';
import { provideAutoSpy } from 'jest-auto-spies';
import { ApiHttpClient } from '../resources/api-http-client.service';

describe('ClientLogsService', () => {
  let service: ClientLogsService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [provideAutoSpy(ApiHttpClient)],
    });
    service = TestBed.inject(ClientLogsService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
