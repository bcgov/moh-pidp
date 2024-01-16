import { TestBed } from '@angular/core/testing';

import { provideAutoSpy } from 'jest-auto-spies';

import { ApiHttpClient } from '@app/core/resources/api-http-client.service';
import { ToastService } from '@app/core/services/toast.service';

import { EndorsementsResource } from './endorsements-resource.service';

describe('EndorsementsResourceService', () => {
  let service: EndorsementsResource;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [provideAutoSpy(ApiHttpClient), provideAutoSpy(ToastService)],
    });
    service = TestBed.inject(EndorsementsResource);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
