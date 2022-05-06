import { TestBed } from '@angular/core/testing';

import { provideAutoSpy } from 'jest-auto-spies';

import { ApiHttpClient } from '@app/core/resources/api-http-client.service';
import { ToastService } from '@app/core/services/toast.service';

import { WorkAndRoleInformationResource } from './work-and-role-information-resource.service';

describe('WorkAndRoleInformationResource', () => {
  let service: WorkAndRoleInformationResource;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        WorkAndRoleInformationResource,
        provideAutoSpy(ApiHttpClient),
        provideAutoSpy(ToastService),
      ],
    });

    service = TestBed.inject(WorkAndRoleInformationResource);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
