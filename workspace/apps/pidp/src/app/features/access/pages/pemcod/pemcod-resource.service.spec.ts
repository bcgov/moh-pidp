import { HttpClient } from '@angular/common/http';
import { TestBed } from '@angular/core/testing';

import { provideAutoSpy } from 'jest-auto-spies';

import { APP_CONFIG, APP_DI_CONFIG } from '@app/app.config';

import { PemcodResource } from './pemcod-resource.service';

describe('PemcodResource', () => {
  let service: PemcodResource;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        PemcodResource,
        {
          provide: APP_CONFIG,
          useValue: APP_DI_CONFIG,
        },
        provideAutoSpy(HttpClient),
      ],
    });
    service = TestBed.inject(PemcodResource);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
