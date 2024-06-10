import { TestBed } from '@angular/core/testing';

import { provideAutoSpy } from 'jest-auto-spies';

import { ApiHttpClient } from '@app/core/resources/api-http-client.service';
import { ToastService } from '@app/core/services/toast.service';
import { PortalResource } from '@app/features/portal/portal-resource.service';

import { MsTeamsClinicMemberResource } from './ms-teams-clinic-member-resource.service';

describe('MsTeamsClinicMemberResource', () => {
  let service: MsTeamsClinicMemberResource;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        provideAutoSpy(ApiHttpClient),
        provideAutoSpy(PortalResource),
        provideAutoSpy(ToastService),
      ],
    });
    service = TestBed.inject(MsTeamsClinicMemberResource);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
