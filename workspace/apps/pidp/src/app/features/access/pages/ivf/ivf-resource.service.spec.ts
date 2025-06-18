import { HttpClient } from '@angular/common/http';
import { TestBed } from '@angular/core/testing';

import { provideAutoSpy } from 'jest-auto-spies';

import { APP_CONFIG, APP_DI_CONFIG } from '@app/app.config';

import { IvfResource } from './ivf-resource.service';

describe('IvfResource', () => {
  let service: IvfResource;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        IvfResource,
        {
          provide: APP_CONFIG,
          useValue: APP_DI_CONFIG,
        },
        provideAutoSpy(HttpClient),
      ],
    });
    service = TestBed.inject(IvfResource);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
