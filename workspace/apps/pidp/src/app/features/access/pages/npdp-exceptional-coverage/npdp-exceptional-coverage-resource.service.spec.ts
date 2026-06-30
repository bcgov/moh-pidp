import { HttpClient } from '@angular/common/http';
import { TestBed } from '@angular/core/testing';

import { provideAutoSpy } from 'jest-auto-spies';

import { APP_CONFIG, APP_DI_CONFIG } from '@app/app.config';

import { NpdpExceptionalCoverageResource } from './npdp-exceptional-coverage-resource.service';

describe('NpdpExceptionalCoverageResource', () => {
  let service: NpdpExceptionalCoverageResource;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        NpdpExceptionalCoverageResource,
        {
          provide: APP_CONFIG,
          useValue: APP_DI_CONFIG,
        },
        provideAutoSpy(HttpClient),
      ],
    });
    service = TestBed.inject(NpdpExceptionalCoverageResource);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
