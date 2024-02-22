/* eslint-disable @typescript-eslint/no-explicit-any */
import { TestBed } from '@angular/core/testing';
import { ActivatedRoute, Router } from '@angular/router';

import { randTextRange } from '@ngneat/falso';
import { provideAutoSpy } from 'jest-auto-spies';

import { SafePipe } from '@bcgov/shared/ui';

import { APP_CONFIG, APP_DI_CONFIG } from '@app/app.config';
import { PartyService } from '@app/core/party/party.service';
import { DocumentService } from '@app/core/services/document.service';
import { LoggerService } from '@app/core/services/logger.service';

import { PrescriptionRefillEformsResource } from './prescription-refill-eforms-resource.service';
import { PrescriptionRefillEformsPage } from './prescription-refill-eforms.page';

describe('PrescriptionRefillEformsPage', () => {
  let component: PrescriptionRefillEformsPage;

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
      imports: [SafePipe],
      providers: [
        PrescriptionRefillEformsPage,
        {
          provide: ActivatedRoute,
          useValue: mockActivatedRoute,
        },
        {
          provide: APP_CONFIG,
          useValue: APP_DI_CONFIG,
        },
        provideAutoSpy(Router),
        provideAutoSpy(PartyService),
        provideAutoSpy(PrescriptionRefillEformsResource),
        provideAutoSpy(LoggerService),
        provideAutoSpy(DocumentService),
      ],
    });

    component = TestBed.inject(PrescriptionRefillEformsPage);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
