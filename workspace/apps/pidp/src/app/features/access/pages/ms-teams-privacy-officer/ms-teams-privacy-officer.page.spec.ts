import { TestBed } from '@angular/core/testing';
import { provideAutoSpy } from 'jest-auto-spies';
import { randTextRange } from '@ngneat/falso';

import { MatDialogModule } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder } from '@angular/forms';

import { MsTeamsPrivacyOfficerPage } from './ms-teams-privacy-officer.page';
import { MsTeamsPrivacyOfficerResource } from './ms-teams-privacy-officer-resource.service';
import { FormUtilsService } from '@app/core/services/form-utils.service';
import { APP_CONFIG, APP_DI_CONFIG } from '@app/app.config';

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
      imports: [
        MatDialogModule,
      ],
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
