/* eslint-disable @typescript-eslint/no-explicit-any */
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Router, RouterModule } from '@angular/router';

import { randNumber, randText } from '@ngneat/falso';
import { MockProfileStatus } from '@test/mock-profile-status';
import {
  Spy,
  createFunctionSpy,
  createSpyFromClass,
  provideAutoSpy,
} from 'jest-auto-spies';

import { APP_CONFIG, APP_DI_CONFIG } from '@app/app.config';
import { PartyService } from '@app/core/party/party.service';
import { ApiHttpClient } from '@app/core/resources/api-http-client.service';
import { AuthService } from '@app/features/auth/services/auth.service';

import { StatusCode } from '../../enums/status-code.enum';
import { ProfileStatus } from '../../models/profile-status.model';
import { IPortalSection } from '../../state/portal-section.model';
import { PortalCardComponent } from './portal-card.component';

describe('PortalCardComponent', () => {
  let component: PortalCardComponent;
  let partyServiceSpy: Spy<PartyService>;
  let fixture: ComponentFixture<PortalCardComponent>;
  let router: Router;
  let mockProfileStatus: ProfileStatus;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RouterModule.forRoot([])],
      providers: [
        provideAutoSpy(ApiHttpClient),
        {
          provide: PartyService,
          useValue: createSpyFromClass(PartyService, {
            gettersToSpyOn: ['partyId'],
            settersToSpyOn: ['partyId'],
          }),
        },
        provideAutoSpy(Router),
        {
          provide: APP_CONFIG,
          useValue: APP_DI_CONFIG,
        },
        provideAutoSpy(AuthService),
      ],
    }).compileComponents();

    partyServiceSpy = TestBed.inject<any>(PartyService);
    router = TestBed.inject(Router);

    fixture = TestBed.createComponent(PortalCardComponent);
    component = fixture.componentInstance;

    mockProfileStatus = MockProfileStatus.get();
    mockProfileStatus.status.provincialAttachmentSystem.statusCode =
      StatusCode.NOT_AVAILABLE;

    jest.spyOn(window, 'open').mockImplementation(() => null);
  });

  describe('METHOD: onClickVisit', () => {
    given('the component has been initialized', () => {
      const partyId = randNumber({ min: 1 });
      partyServiceSpy.accessorSpies.getters.partyId.mockReturnValue(partyId);
      type PerformAction = IPortalSection['performAction'];
      const performActionSpy =
        createFunctionSpy<PerformAction>('performAction');

      const section = {
        key: 'demographics',
        heading: randText(),
        hint: randText(),
        description: randText(),
        properties: [],
        action: {
          label: '',
          route: '',
          disabled: false,
        },
        statusType: 'success',
        status: randText(),
        performAction: performActionSpy,
      } as IPortalSection;

      when('the onClickVisit method is invoked', () => {
        component.onClickVisit(section);

        then('the router should navigate', () => {
          expect(router.navigateByUrl).toHaveBeenCalledWith('/');
        });
      });
    });
  });
});
