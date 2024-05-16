/* eslint-disable @typescript-eslint/no-explicit-any */
import { TestBed } from '@angular/core/testing';
import { Router } from '@angular/router';
import { RouterTestingModule } from '@angular/router/testing';
import { MockProfileStatus } from '@test/mock-profile-status';
import {
  createSpyFromClass,
  provideAutoSpy,
} from 'jest-auto-spies';

import { APP_CONFIG, APP_DI_CONFIG } from '@app/app.config';
import { PartyService } from '@app/core/party/party.service';
import { ApiHttpClient } from '@app/core/resources/api-http-client.service';
import { AuthService } from '@app/features/auth/services/auth.service';

import { StatusCode } from '@app/features/portal/enums/status-code.enum';
import { ProfileStatus } from '@app/features/portal/models/profile-status.model';

describe('PortalCardComponent', () => {
  let mockProfileStatus: ProfileStatus;


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
 
    mockProfileStatus = MockProfileStatus.get();
    mockProfileStatus.status.primaryCareRostering.statusCode =
      StatusCode.NOT_AVAILABLE;
  });
});