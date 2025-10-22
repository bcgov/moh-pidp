/* eslint-disable @typescript-eslint/no-explicit-any */
import { HttpClient } from '@angular/common/http';
import { TestBed } from '@angular/core/testing';
import { ActivatedRoute, Router } from '@angular/router';

import { randTextRange } from '@ngneat/falso';
import { createSpyFromClass, provideAutoSpy } from 'jest-auto-spies';
import { KeycloakService } from 'keycloak-angular';

import { APP_CONFIG, APP_DI_CONFIG } from '@app/app.config';
import { DiscoveryResource } from '@app/core/party/discovery-resource.service';
import { PartyService } from '@app/core/party/party.service';
import { AuthService } from '@app/features/auth/services/auth.service';
import { PortalResource } from '@app/features/portal/portal-resource.service';
import { PortalService } from '@app/features/portal/portal.service';

import { BcProviderEditResource } from '../../../accounts/pages/bc-provider-edit/bc-provider-edit-resource.service';
import { IvfPage } from './ivf.page';

describe('IvfPage', () => {
  let component: IvfPage;
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
        IvfPage,
        { provide: APP_CONFIG, useValue: APP_DI_CONFIG },
        provideAutoSpy(KeycloakService),
        provideAutoSpy(HttpClient),
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
    component = TestBed.inject(IvfPage);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
