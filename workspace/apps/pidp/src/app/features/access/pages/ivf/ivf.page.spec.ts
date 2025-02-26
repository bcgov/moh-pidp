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
import { IvfPage } from './ivf.page';
import { DocumentService } from '@app/core/services/document.service';
import { LoggerService } from '@app/core/services/logger.service';
import { IvfResource } from './ivf-resource.service';

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
        provideAutoSpy(IvfResource),
        provideAutoSpy(LoggerService),
        provideAutoSpy(DocumentService),
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
