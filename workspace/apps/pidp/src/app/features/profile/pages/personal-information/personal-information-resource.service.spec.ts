import { TestBed } from '@angular/core/testing';

import { provideAutoSpy } from 'jest-auto-spies';

import { ApiHttpClient } from '@app/core/resources/api-http-client.service';
import { ToastService } from '@app/core/services/toast.service';

import { PersonalInformationResource } from './personal-information-resource.service';

describe('ProfileInformationResource', () => {
  let service: PersonalInformationResource;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        PersonalInformationResource,
        provideAutoSpy(ApiHttpClient),
        provideAutoSpy(ToastService),
      ],
    });
    service = TestBed.inject(PersonalInformationResource);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
