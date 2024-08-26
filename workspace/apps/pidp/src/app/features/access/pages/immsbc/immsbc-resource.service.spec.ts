import { HttpClient } from '@angular/common/http';
import { TestBed } from '@angular/core/testing';

import { provideAutoSpy } from 'jest-auto-spies';

import { APP_CONFIG, APP_DI_CONFIG } from '@app/app.config';
import { ApiHttpClient } from '@app/core/resources/api-http-client.service';

import { ImmsBCResource } from './immsbc-resource.service';

describe('ImmsBCResource', () => {
  let service: ImmsBCResource;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        ImmsBCResource,
        {
          provide: APP_CONFIG,
          useValue: APP_DI_CONFIG,
        },
        provideAutoSpy(HttpClient),
        provideAutoSpy(ApiHttpClient),
      ],
    });
    service = TestBed.inject(ImmsBCResource);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
