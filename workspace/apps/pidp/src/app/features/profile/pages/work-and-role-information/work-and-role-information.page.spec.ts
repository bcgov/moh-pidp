import { TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { MatDialogModule } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { RouterTestingModule } from '@angular/router/testing';

import { randTextRange } from '@ngneat/falso';
import { provideAutoSpy } from 'jest-auto-spies';

import { APP_CONFIG, APP_DI_CONFIG } from '@app/app.config';
import { FormUtilsService } from '@app/core/services/form-utils.service';
import { LoggerService } from '@app/core/services/logger.service';

import { WorkAndRoleInformationResource } from './work-and-role-information-resource.service';
import { WorkAndRoleInformationPage } from './work-and-role-information.page';

describe('WorkAndRoleInformationPage', () => {
  let component: WorkAndRoleInformationPage;

  const mockActivatedRoute = {
    snapshot: {
      data: {
        title: randTextRange({ min: 1, max: 4 }),
        routes: {
          root: '../../',
        },
      },
    },
  };

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [MatDialogModule, ReactiveFormsModule, RouterTestingModule],
      providers: [
        WorkAndRoleInformationPage,
        {
          provide: APP_CONFIG,
          useValue: APP_DI_CONFIG,
        },
        {
          provide: ActivatedRoute,
          useValue: mockActivatedRoute,
        },
        provideAutoSpy(WorkAndRoleInformationResource),
        provideAutoSpy(FormUtilsService),
        provideAutoSpy(LoggerService),
        provideAutoSpy(Router),
      ],
    });

    component = TestBed.inject(WorkAndRoleInformationPage);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
