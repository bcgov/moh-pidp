import { TestBed } from '@angular/core/testing';

import { provideAutoSpy } from 'jest-auto-spies';

import { ApiHttpClient } from '@app/core/resources/api-http-client.service';
import { PortalResource } from '@app/features/portal/portal-resource.service';

import { SaEformsResource } from './sa-eforms-resource.service';

describe('SaEformsResource', () => {
  let service: SaEformsResource;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        SaEformsResource,
        provideAutoSpy(ApiHttpClient),
        provideAutoSpy(PortalResource),
      ],
    });
    service = TestBed.inject(SaEformsResource);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
