/* eslint-disable @typescript-eslint/no-explicit-any */
import { TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { MatDialogModule } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';

import { randTextRange } from '@ngneat/falso';
import { provideAutoSpy } from 'jest-auto-spies';

import { APP_CONFIG, APP_DI_CONFIG } from '@app/app.config';

import { MsTeamsClinicMemberResource } from './ms-teams-clinic-member-resource.service';
import { MsTeamsClinicMemberPage } from './ms-teams-clinic-member.page';

describe('MsTeamsClinicMemberPage', () => {
  let component: MsTeamsClinicMemberPage;

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
      imports: [MatDialogModule, ReactiveFormsModule],
      providers: [
        MsTeamsClinicMemberPage,
        {
          provide: APP_CONFIG,
          useValue: APP_DI_CONFIG,
        },
        {
          provide: ActivatedRoute,
          useValue: mockActivatedRoute,
        },
        provideAutoSpy(Router),
        provideAutoSpy(MsTeamsClinicMemberResource),
      ],
    });

    component = TestBed.inject(MsTeamsClinicMemberPage);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
