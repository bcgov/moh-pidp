import { HttpClient } from '@angular/common/http';
import { TestBed } from '@angular/core/testing';

import { provideAutoSpy } from 'jest-auto-spies';

import { APP_CONFIG, APP_DI_CONFIG } from '@app/app.config';

import { ImmsbcResource } from './immsbc-resource.service';

describe('ImmsbcResource', () => {
  let service: ImmsbcResource;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        ImmsbcResource,
        {
          provide: APP_CONFIG,
          useValue: APP_DI_CONFIG,
        },
        provideAutoSpy(HttpClient),
      ],
    });
    service = TestBed.inject(ImmsbcResource);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
