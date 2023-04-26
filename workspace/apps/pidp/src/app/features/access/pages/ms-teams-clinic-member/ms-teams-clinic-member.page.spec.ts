/* eslint-disable @typescript-eslint/no-explicit-any */
import { TestBed } from '@angular/core/testing';
import { provideAutoSpy } from 'jest-auto-spies';
import { randTextRange } from '@ngneat/falso';

import { ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { MatDialogModule } from '@angular/material/dialog';

import { MsTeamsClinicMemberPage } from './ms-teams-clinic-member.page';
import { MsTeamsClinicMemberResource } from './ms-teams-clinic-member-resource.service';
import { APP_CONFIG, APP_DI_CONFIG } from '@app/app.config';

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
      imports: [
        MatDialogModule,
        ReactiveFormsModule,
      ],
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
      ]
    });

    component = TestBed.inject(MsTeamsClinicMemberPage);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
