import { HttpClient } from '@angular/common/http';
import { TestBed } from '@angular/core/testing';

import { provideAutoSpy } from 'jest-auto-spies';

import { APP_CONFIG, APP_DI_CONFIG } from '@app/app.config';

import { PHRResource } from './phr-resource.service';

describe('PHRResource', () => {
  let service: PHRResource;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        PHRResource,
        {
          provide: APP_CONFIG,
          useValue: APP_DI_CONFIG,
        },
        provideAutoSpy(HttpClient),
      ],
    });
    service = TestBed.inject(PHRResource);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
