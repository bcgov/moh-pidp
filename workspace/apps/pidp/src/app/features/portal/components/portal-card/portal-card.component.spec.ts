/* eslint-disable @typescript-eslint/no-explicit-any */
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Router } from '@angular/router';
import { RouterTestingModule } from '@angular/router/testing';

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
  let windowSpy: Spy<any>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RouterTestingModule],
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

    windowSpy = jest.spyOn(window, 'window', 'get');
  });

  describe('METHOD: onClickVisit', () => {
    given('the component has been initialized', () => {
      const partyId = randNumber({ min: 1 });
      partyServiceSpy.accessorSpies.getters.partyId.mockReturnValue(partyId);
      const performAction = (): void => {
        return;
      };

      const performActionSpy =
        createFunctionSpy<typeof performAction>('performAction');

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
        windowSpy.mockImplementation(() => ({
          open: (): void => {
            return;
          },
        }));
        component.onClickVisit(section);

        then('the router should navigate', () => {
          expect(router.navigateByUrl).toHaveBeenCalledWith('/');
        });
      });
    });
  });
});
