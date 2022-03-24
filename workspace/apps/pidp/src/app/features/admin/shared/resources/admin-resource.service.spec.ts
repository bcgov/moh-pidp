import { TestBed } from '@angular/core/testing';

import { provideAutoSpy } from 'jest-auto-spies';

import { ApiHttpClient } from '@app/core/resources/api-http-client.service';

import { AdminResource } from './admin-resource.service';

describe('AdminResource', () => {
  let service: AdminResource;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [AdminResource, provideAutoSpy(ApiHttpClient)],
    });

    service = TestBed.inject(AdminResource);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
