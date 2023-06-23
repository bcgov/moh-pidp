import { TestBed } from '@angular/core/testing';

import { provideAutoSpy } from 'jest-auto-spies';

import { ApiHttpClient } from '@app/core/resources/api-http-client.service';
import { ToastService } from '@app/core/services/toast.service';

import { UserAccessAgreementResource } from './user-access-agreement-resource.service';

describe('UserAccessAgreementResource', () => {
  let service: UserAccessAgreementResource;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        UserAccessAgreementResource,
        provideAutoSpy(ApiHttpClient),
        provideAutoSpy(ToastService),
      ],
    });
    service = TestBed.inject(UserAccessAgreementResource);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
