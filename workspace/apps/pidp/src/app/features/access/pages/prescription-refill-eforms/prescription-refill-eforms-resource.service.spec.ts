import { TestBed } from '@angular/core/testing';

import { provideAutoSpy } from 'jest-auto-spies';

import { ApiHttpClient } from '@app/core/resources/api-http-client.service';
import { PortalResource } from '@app/features/portal/portal-resource.service';

import { PrescriptionRefillEformsResource } from './prescription-refill-eforms-resource.service';

describe('PrescriptionRefillEformsResource', () => {
  let service: PrescriptionRefillEformsResource;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        provideAutoSpy(ApiHttpClient),
        provideAutoSpy(PortalResource),
      ],
    });
    service = TestBed.inject(PrescriptionRefillEformsResource);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
