import { TestBed } from '@angular/core/testing';

import { provideAutoSpy } from 'jest-auto-spies';

import { ApiHttpClient } from '@app/core/resources/api-http-client.service';
import { PortalResource } from '@app/features/portal/portal-resource.service';

import { HaloResource } from './halo-resource.service';

describe('HaloResource', () => {
  let service: HaloResource;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        provideAutoSpy(ApiHttpClient),
        provideAutoSpy(PortalResource),
      ],
    });
    service = TestBed.inject(HaloResource);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
