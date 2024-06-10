/* eslint-disable @typescript-eslint/no-explicit-any */
import { TestBed } from '@angular/core/testing';
import { ActivatedRoute, Router } from '@angular/router';

import { randTextRange } from '@ngneat/falso';
import { createSpyFromClass, provideAutoSpy } from 'jest-auto-spies';

import { PartyService } from '@app/core/party/party.service';
import { DocumentService } from '@app/core/services/document.service';
import { LoggerService } from '@app/core/services/logger.service';

import { ProviderReportingPortalResource } from './provider-reporting-portal-resource.service';
import { ProviderReportingPortalPage } from './provider-reporting-portal.page';

describe('ProviderReportingPortalPage', () => {
  let component: ProviderReportingPortalPage;

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
        ProviderReportingPortalPage,
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
        provideAutoSpy(Router),
        provideAutoSpy(ProviderReportingPortalResource),
        provideAutoSpy(LoggerService),
        provideAutoSpy(DocumentService),
      ],
    });

    component = TestBed.inject(ProviderReportingPortalPage);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
