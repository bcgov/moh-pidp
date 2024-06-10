/* eslint-disable @typescript-eslint/no-explicit-any */
import { TestBed } from '@angular/core/testing';

import { DashboardStateModel, PidpStateName } from '@pidp/data-model';
import { AppStateService } from '@pidp/presentation';
import { MockProfileStatus } from '@test/mock-profile-status';
import { Spy, createSpyFromClass, provideAutoSpy } from 'jest-auto-spies';

import { PartyService } from '@app/core/party/party.service';
import { ProfileStatus } from '@app/features/portal/models/profile-status.model';
import { PortalResource } from '@app/features/portal/portal-resource.service';
import { LookupService } from '@app/modules/lookup/lookup.service';

import { DashboardStateService } from './dashboard-state-service.service';

describe('DashboardStateService', () => {
  let appStateService: Spy<AppStateService>;
  let lookupServiceSpy: Spy<LookupService>;
  let portalResourceService: Spy<PortalResource>;
  let service: DashboardStateService;

  let mockProfileStatus: ProfileStatus;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        DashboardStateService,
        provideAutoSpy(AppStateService),
        provideAutoSpy(LookupService),
        {
          provide: PartyService,
          useValue: createSpyFromClass(PartyService, {
            gettersToSpyOn: ['partyId'],
            settersToSpyOn: ['partyId'],
          }),
        },
        provideAutoSpy(PortalResource),
      ],
    });
    service = TestBed.inject(DashboardStateService);

    appStateService = TestBed.inject<any>(AppStateService);
    lookupServiceSpy = TestBed.inject<any>(LookupService);
    portalResourceService = TestBed.inject<any>(PortalResource);

    mockProfileStatus = MockProfileStatus.get();
    mockProfileStatus.status.dashboardInfo.displayFullName = 'John Doe';
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  describe('METHOD refreshDashboardState', () => {
    given('this is the first initialize', () => {
      portalResourceService.getProfileStatus.nextOneTimeWith(mockProfileStatus);
      appStateService.getNamedState
        .calledWith(PidpStateName.dashboard)
        .mockReturnValue({
          stateName: PidpStateName.dashboard,
          titleText: 'Welcome to Test page',
          titleDescriptionText: 'This is a description text for unit test',
          userProfileFullNameText: '',
          userProfileCollegeNameText: '',
        } as DashboardStateModel);

      lookupServiceSpy.getCollege.mockReturnValue({
        code: 2,
        name: 'College of Pharmacists of BC',
      });

      when('refreshDashboardState is called', () => {
        service.refreshDashboardState();

        then(
          'user name and college name are set in the AppStateService',
          () => {
            appStateService.setNamedState.mustBeCalledWith(
              PidpStateName.dashboard,
              {
                stateName: PidpStateName.dashboard,
                titleText: 'Welcome to Test page',
                titleDescriptionText:
                  'This is a description text for unit test',
                userProfileFullNameText: 'John Doe',
                userProfileCollegeNameText: 'College of Pharmacists of BC',
              } as DashboardStateModel,
            );
          },
        );
      });
    });

    given('the profile status is null', () => {
      portalResourceService.getProfileStatus.nextOneTimeWith(null);
      appStateService.getNamedState
        .calledWith(PidpStateName.dashboard)
        .mockReturnValue({
          stateName: PidpStateName.dashboard,
          titleText: 'Welcome to Test page',
          titleDescriptionText: 'This is a description text for unit test',
          userProfileFullNameText: '',
          userProfileCollegeNameText: '',
        } as DashboardStateModel);

      lookupServiceSpy.getCollege.mockReturnValue({
        code: 2,
        name: 'College of Pharmacists of BC',
      });

      when('refreshDashboardState is called', () => {
        service.refreshDashboardState();

        then('user name and college name are set to empty string', () => {
          appStateService.setNamedState.mustBeCalledWith(
            PidpStateName.dashboard,
            {
              stateName: PidpStateName.dashboard,
              titleText: 'Welcome to Test page',
              titleDescriptionText: 'This is a description text for unit test',
              userProfileFullNameText: '',
              userProfileCollegeNameText: '',
            } as DashboardStateModel,
          );
        });
      });
    });

    given('the college code is not filled in', () => {
      mockProfileStatus.status.dashboardInfo.collegeCode = undefined;
      portalResourceService.getProfileStatus.nextOneTimeWith(mockProfileStatus);
      appStateService.getNamedState
        .calledWith(PidpStateName.dashboard)
        .mockReturnValue({
          stateName: PidpStateName.dashboard,
          titleText: 'Welcome to Test page',
          titleDescriptionText: 'This is a description text for unit test',
          userProfileFullNameText: '',
          userProfileCollegeNameText: '',
        } as DashboardStateModel);

      when('refreshDashboardState is called', () => {
        service.refreshDashboardState();

        then('college name is set to empty string', () => {
          appStateService.setNamedState.mustBeCalledWith(
            PidpStateName.dashboard,
            {
              stateName: PidpStateName.dashboard,
              titleText: 'Welcome to Test page',
              titleDescriptionText: 'This is a description text for unit test',
              userProfileFullNameText: 'John Doe',
              userProfileCollegeNameText: '',
            } as DashboardStateModel,
          );
        });
      });
    });
  });
});
