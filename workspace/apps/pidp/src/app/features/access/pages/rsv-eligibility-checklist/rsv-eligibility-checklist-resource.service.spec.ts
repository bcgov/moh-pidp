import { HttpClient } from '@angular/common/http';
import { TestBed } from '@angular/core/testing';

import { provideAutoSpy } from 'jest-auto-spies';

import { APP_CONFIG, APP_DI_CONFIG } from '@app/app.config';

import { RsvEligibilityChecklistResource } from './rsv-eligibility-checklist-resource.service';

describe('RsvEligibilityChecklistResource', () => {
  let service: RsvEligibilityChecklistResource;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        RsvEligibilityChecklistResource,
        {
          provide: APP_CONFIG,
          useValue: APP_DI_CONFIG,
        },
        provideAutoSpy(HttpClient),
      ],
    });
    service = TestBed.inject(RsvEligibilityChecklistResource);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
