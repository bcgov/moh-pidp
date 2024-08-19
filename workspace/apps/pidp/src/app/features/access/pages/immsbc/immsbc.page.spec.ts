/* eslint-disable @typescript-eslint/no-explicit-any */
import { TestBed } from '@angular/core/testing';
import { ActivatedRoute, Router } from '@angular/router';

import { randTextRange } from '@ngneat/falso';
import { createSpyFromClass, provideAutoSpy } from 'jest-auto-spies';

import { APP_CONFIG, APP_DI_CONFIG } from '@app/app.config';
import { DiscoveryResource } from '@app/core/party/discovery-resource.service';
import { PartyService } from '@app/core/party/party.service';
import { AuthService } from '@app/features/auth/services/auth.service';
import { PortalResource } from '@app/features/portal/portal-resource.service';
import { PortalService } from '@app/features/portal/portal.service';

import { BcProviderEditResource } from '../bc-provider-edit/bc-provider-edit-resource.service';
import { ImmsbcPage } from './immsbc.page';

describe('ImmsbcPage', () => {
  let component: ImmsbcPage;

  let mockActivatedRoute: { snapshot: any };

  beforeEach(() => {
    mockActivatedRoute = {
      snapshot: {
        data: {
          title: randTextRange({ min: 1, max: 4 }),
          routes: {
            root: '../../',
          },
        },
      },
    };
    TestBed.configureTestingModule({
      providers: [
        ImmsbcPage,
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
        provideAutoSpy(AuthService),
        provideAutoSpy(Router),
        provideAutoSpy(PortalResource),
        provideAutoSpy(PortalService),
        provideAutoSpy(DiscoveryResource),
        provideAutoSpy(BcProviderEditResource),
      ],
    });
    component = TestBed.inject(ImmsbcPage);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
