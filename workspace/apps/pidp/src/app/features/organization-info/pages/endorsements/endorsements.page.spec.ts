import { TestBed } from '@angular/core/testing';
import { provideAutoSpy } from 'jest-auto-spies';
import { randTextRange } from '@ngneat/falso';

import { MatDialogModule } from '@angular/material/dialog';
import { ActivatedRoute } from '@angular/router';
import { FormBuilder } from '@angular/forms';

import { EndorsementsPage } from './endorsements.page';
import { EndorsementsResource } from './endorsements-resource.service';
import { FormUtilsService } from '@app/core/services/form-utils.service';
import { LookupService } from '@app/modules/lookup/lookup.service';
import { NavigationService } from '@pidp/presentation';
import { APP_CONFIG, APP_DI_CONFIG } from '@app/app.config';

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
      imports: [
        MatDialogModule,
      ],
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
      ]
    });

    component = TestBed.inject(EndorsementsPage);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
