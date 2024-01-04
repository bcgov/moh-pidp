/* eslint-disable @typescript-eslint/no-explicit-any */
import { TestBed } from '@angular/core/testing';
import { FormBuilder } from '@angular/forms';
import { MatDialogModule } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';

import { randTextRange } from '@ngneat/falso';
import { provideAutoSpy } from 'jest-auto-spies';

import { APP_CONFIG, APP_DI_CONFIG } from '@app/app.config';
import { FormUtilsService } from '@app/core/services/form-utils.service';

import { MsTeamsPrivacyOfficerResource } from './ms-teams-privacy-officer-resource.service';
import { MsTeamsPrivacyOfficerPage } from './ms-teams-privacy-officer.page';

describe('MsTeamsPrivacyOfficerPage', () => {
  let component: MsTeamsPrivacyOfficerPage;

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
        MsTeamsPrivacyOfficerPage,
        {
          provide: APP_CONFIG,
          useValue: APP_DI_CONFIG,
        },
        {
          provide: ActivatedRoute,
          useValue: mockActivatedRoute,
        },
        provideAutoSpy(Router),
        provideAutoSpy(FormUtilsService),
        provideAutoSpy(FormBuilder),
        provideAutoSpy(MsTeamsPrivacyOfficerResource),
      ],
    });

    component = TestBed.inject(MsTeamsPrivacyOfficerPage);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
