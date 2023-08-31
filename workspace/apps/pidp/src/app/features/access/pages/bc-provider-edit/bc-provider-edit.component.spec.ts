/* eslint-disable @typescript-eslint/no-explicit-any */
import { HttpClient } from '@angular/common/http';
import { TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { MatDialogModule } from '@angular/material/dialog';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';

import { randTextRange } from '@ngneat/falso';
import { provideAutoSpy } from 'jest-auto-spies';

import { APP_CONFIG, APP_DI_CONFIG } from '@app/app.config';

import { BcProviderEditComponent } from './bc-provider-edit.component';

describe('BcProviderApplicationComponent', () => {
  let component: BcProviderEditComponent;

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
      imports: [MatDialogModule, MatSnackBarModule, ReactiveFormsModule],
      providers: [
        BcProviderEditComponent,
        { provide: APP_CONFIG, useValue: APP_DI_CONFIG },
        {
          provide: ActivatedRoute,
          useValue: mockActivatedRoute,
        },
        provideAutoSpy(HttpClient),
        provideAutoSpy(Router),
      ],
    });
    component = TestBed.inject(BcProviderEditComponent);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
