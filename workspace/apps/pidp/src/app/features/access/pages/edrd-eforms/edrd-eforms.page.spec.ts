/* eslint-disable @typescript-eslint/no-explicit-any */
import { TestBed } from '@angular/core/testing';
import { ActivatedRoute, Router } from '@angular/router';

import { randTextRange } from '@ngneat/falso';
import { provideAutoSpy } from 'jest-auto-spies';

import { APP_CONFIG, APP_DI_CONFIG } from '@app/app.config';
import { PartyService } from '@app/core/party/party.service';
import { DocumentService } from '@app/core/services/document.service';
import { LoggerService } from '@app/core/services/logger.service';

import { EdrdEformsPage } from './edrd-eforms.page';
import { EdredEformsResource } from './edred-eforms-resource.service';

describe('EdrdEformsPage', () => {
  let component: EdrdEformsPage;

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
        EdrdEformsPage,
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
        provideAutoSpy(EdredEformsResource),
        provideAutoSpy(LoggerService),
        provideAutoSpy(DocumentService),
      ],
    });

    component = TestBed.inject(EdrdEformsPage);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
