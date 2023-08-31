/* eslint-disable @typescript-eslint/no-explicit-any */
import { HttpClient } from '@angular/common/http';
import { TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { MatDialogModule } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';

import { randTextRange } from '@ngneat/falso';
import { provideAutoSpy } from 'jest-auto-spies';
import { KeycloakService } from 'keycloak-angular';

import { APP_CONFIG, APP_DI_CONFIG } from '@app/app.config';

import { BcProviderApplicationComponent } from './bc-provider-application.component';

describe('BcProviderApplicationComponent', () => {
  let component: BcProviderApplicationComponent;

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
        BcProviderApplicationComponent,
        { provide: APP_CONFIG, useValue: APP_DI_CONFIG },
        {
          provide: ActivatedRoute,
          useValue: mockActivatedRoute,
        },
        provideAutoSpy(HttpClient),
        provideAutoSpy(KeycloakService),
        provideAutoSpy(Router),
      ],
    });
    component = TestBed.inject(BcProviderApplicationComponent);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
