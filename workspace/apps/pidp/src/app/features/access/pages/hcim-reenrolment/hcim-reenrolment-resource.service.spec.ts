import { TestBed } from '@angular/core/testing';

import { provideAutoSpy } from 'jest-auto-spies';

import { ApiHttpClient } from '@app/core/resources/api-http-client.service';

import { HcimReenrolmentResource } from './hcim-reenrolment-resource.service';

describe('HcimWebEnrolmentResource', () => {
  let service: HcimReenrolmentResource;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [HcimReenrolmentResource, provideAutoSpy(ApiHttpClient)],
    });

    service = TestBed.inject(HcimReenrolmentResource);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
