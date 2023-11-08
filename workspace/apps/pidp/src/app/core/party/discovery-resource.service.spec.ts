import { TestBed } from '@angular/core/testing';
import { HttpClient } from '@angular/common/http';

import { provideAutoSpy } from 'jest-auto-spies';

import { AuthorizedUserService } from '@app/features/auth/services/authorized-user.service';
import { APP_CONFIG, APP_DI_CONFIG } from '@app/app.config';

import { ApiHttpClient } from '../resources/api-http-client.service';
import { DiscoveryResource } from './discovery-resource.service';

describe('DiscoveryResource', () => {
  let service: DiscoveryResource;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        provideAutoSpy(ApiHttpClient),
        {
          provide: APP_CONFIG,
          useValue: APP_DI_CONFIG,
        },
        provideAutoSpy(HttpClient),
        provideAutoSpy(AuthorizedUserService),
      ],
    });

    service = TestBed.inject(DiscoveryResource);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
