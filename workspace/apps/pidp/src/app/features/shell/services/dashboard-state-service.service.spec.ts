import { TestBed } from '@angular/core/testing';

import { AppStateService } from '@pidp/presentation';
import { provideAutoSpy } from 'jest-auto-spies';

import { PartyService } from '@app/core/party/party.service';
import { PortalResource } from '@app/features/portal/portal-resource.service';
import { LookupService } from '@app/modules/lookup/lookup.service';

import { DashboardStateService } from './dashboard-state-service.service';

describe('DashboardStateService', () => {
  let service: DashboardStateService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        DashboardStateService,
        provideAutoSpy(PartyService),
        provideAutoSpy(PortalResource),
        provideAutoSpy(LookupService),
        provideAutoSpy(AppStateService),
      ],
    });
    service = TestBed.inject(DashboardStateService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
