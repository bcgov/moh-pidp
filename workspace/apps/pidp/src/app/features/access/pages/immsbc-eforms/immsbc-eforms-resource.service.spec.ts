import { TestBed } from '@angular/core/testing';

import { provideAutoSpy } from 'jest-auto-spies';

import { APP_CONFIG, APP_DI_CONFIG } from '@app/app.config';
import { ApiHttpClient } from '@app/core/resources/api-http-client.service';
import { PortalResource } from '@app/features/portal/portal-resource.service';

import { ImmsBCEformsResource } from './immsbc-eforms-resource.service';

describe('ImmsBCEformsResource', () => {
  let service: ImmsBCEformsResource;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        ImmsBCEformsResource,
        {
          provide: APP_CONFIG,
          useValue: APP_DI_CONFIG,
        },
        provideAutoSpy(ApiHttpClient),
        provideAutoSpy(PortalResource),
      ],
    });
    service = TestBed.inject(ImmsBCEformsResource);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
