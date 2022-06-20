import { TestBed } from '@angular/core/testing';

import { provideAutoSpy } from 'jest-auto-spies';

import { ApiHttpClient } from '@app/core/resources/api-http-client.service';
import { PortalResource } from '@app/features/portal/portal-resource.service';

import { DriverFitnessResource } from './driver-fitness-resource.service';

describe('DriverFitnessResource', () => {
  let service: DriverFitnessResource;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        DriverFitnessResource,
        provideAutoSpy(ApiHttpClient),
        provideAutoSpy(PortalResource),
      ],
    });
    service = TestBed.inject(DriverFitnessResource);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
