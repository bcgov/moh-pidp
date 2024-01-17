/* eslint-disable @typescript-eslint/no-explicit-any */
import { TestBed } from '@angular/core/testing';
import { FormBuilder } from '@angular/forms';
import { MatDialogModule } from '@angular/material/dialog';
import { ActivatedRoute } from '@angular/router';

import { randTextRange } from '@ngneat/falso';
import { NavigationService } from '@pidp/presentation';
import { provideAutoSpy } from 'jest-auto-spies';

import { APP_CONFIG, APP_DI_CONFIG } from '@app/app.config';
import { FormUtilsService } from '@app/core/services/form-utils.service';
import { LookupService } from '@app/modules/lookup/lookup.service';

import { EndorsementsResource } from './endorsements-resource.service';
import { EndorsementsPage } from './endorsements.page';

describe('EndorsementsPage', () => {
  let component: EndorsementsPage;

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
      imports: [MatDialogModule],
      providers: [
        EndorsementsPage,
        {
          provide: APP_CONFIG,
          useValue: APP_DI_CONFIG,
        },
        {
          provide: ActivatedRoute,
          useValue: mockActivatedRoute,
        },
        provideAutoSpy(FormUtilsService),
        provideAutoSpy(EndorsementsResource),
        provideAutoSpy(NavigationService),
        provideAutoSpy(LookupService),
        provideAutoSpy(FormBuilder),
      ],
    });

    component = TestBed.inject(EndorsementsPage);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
