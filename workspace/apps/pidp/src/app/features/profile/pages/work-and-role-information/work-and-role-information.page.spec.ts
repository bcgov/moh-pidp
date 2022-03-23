import { TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { MatDialogModule } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { RouterTestingModule } from '@angular/router/testing';

import {
  randCity,
  randCountryCode,
  randNumber,
  randStateAbbr,
  randStreetAddress,
  randTextRange,
  randZipCode,
} from '@ngneat/falso';
import { Spy, createSpyFromClass, provideAutoSpy } from 'jest-auto-spies';

import { APP_CONFIG, APP_DI_CONFIG } from '@app/app.config';
import { FormUtilsService } from '@app/core/services/form-utils.service';
import { LoggerService } from '@app/core/services/logger.service';
import { PartyService } from '@app/core/services/party.service';

import { WorkAndRoleInformationResource } from './work-and-role-information-resource.service';
import { WorkAndRoleInformationPage } from './work-and-role-information.page';

describe('WorkAndRoleInformationPage', () => {
  let component: WorkAndRoleInformationPage;
  let partyServiceSpy: Spy<PartyService>;
  let workAndRoleInformationResourceSpy: Spy<WorkAndRoleInformationResource>;
  let router: Router;

  const mockActivatedRoute = {
    snapshot: {
      data: {
        title: randTextRange({ min: 1, max: 4 }),
        routes: {
          root: '../../',
        },
      },
    },
  };

  const mockForm = {
    jobTitle: randTextRange({ min: 4, max: 15 }),
    facilityName: randTextRange({ min: 4, max: 15 }),
  };

  const mockParty = {
    facilityAddress: {
      countryCode: randCountryCode(),
      provinceCode: randStateAbbr(),
      street: randStreetAddress(),
      city: randCity(),
      postal: randZipCode(),
    },
    ...mockForm,
  };

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [MatDialogModule, ReactiveFormsModule, RouterTestingModule],
      providers: [
        WorkAndRoleInformationPage,
        {
          provide: APP_CONFIG,
          useValue: APP_DI_CONFIG,
        },
        {
          provide: ActivatedRoute,
          useValue: mockActivatedRoute,
        },
        {
          provide: PartyService,
          useValue: createSpyFromClass(PartyService, {
            gettersToSpyOn: ['partyId'],
            settersToSpyOn: ['partyId'],
          }),
        },
        provideAutoSpy(WorkAndRoleInformationResource),
        provideAutoSpy(FormUtilsService),
        provideAutoSpy(LoggerService),
        provideAutoSpy(Router),
      ],
    });

    component = TestBed.inject(WorkAndRoleInformationPage);
    partyServiceSpy = TestBed.inject<any>(PartyService);
    workAndRoleInformationResourceSpy = TestBed.inject<any>(
      WorkAndRoleInformationResource
    );
    router = TestBed.inject(Router);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  describe('INIT', () => {
    given('partyId exists', () => {
      const partyId = randNumber({ min: 1 });
      partyServiceSpy.accessorSpies.getters.partyId.mockReturnValue(partyId);
      workAndRoleInformationResourceSpy.get.nextOneTimeWith(mockParty);

      when('resource request resolved', () => {
        component.ngOnInit();

        then('it should GET party work and role information', () => {
          expect(router.navigate).not.toHaveBeenCalled();
          expect(workAndRoleInformationResourceSpy.get).toHaveBeenCalledTimes(
            1
          );
          expect(workAndRoleInformationResourceSpy.get).toHaveBeenCalledWith(
            partyId
          );
        });
      });
    });
  });
});
