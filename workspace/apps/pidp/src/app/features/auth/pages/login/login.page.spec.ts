import { TestBed } from '@angular/core/testing';
import { MatDialogModule } from '@angular/material/dialog';
import { ActivatedRoute } from '@angular/router';
import { RouterTestingModule } from '@angular/router/testing';

import { randTextRange } from '@ngneat/falso';
import { provideAutoSpy } from 'jest-auto-spies';

import { APP_CONFIG, APP_DI_CONFIG } from '@app/app.config';
import { DocumentService } from '@app/core/services/document.service';

import { IdentityProvider } from '../../enums/identity-provider.enum';
import { AuthService } from '../../services/auth.service';
import { LoginPageV2Component } from './login.page';

describe('LoginPageV2Component', () => {
  let component: LoginPageV2Component;

  let mockActivatedRoute;

  beforeEach(() => {
    mockActivatedRoute = {
      snapshot: {
        queryParams: { action: '' },
        data: {
          title: randTextRange({ min: 1, max: 4 }),
          idpHint: IdentityProvider.BCSC,
        },
      },
    };

    TestBed.configureTestingModule({
      imports: [RouterTestingModule, MatDialogModule],
      providers: [
        LoginPageV2Component,
        {
          provide: APP_CONFIG,
          useValue: APP_DI_CONFIG,
        },
        {
          provide: ActivatedRoute,
          useValue: mockActivatedRoute,
        },
        provideAutoSpy(AuthService),
        provideAutoSpy(DocumentService),
      ],
    });

    component = TestBed.inject(LoginPageV2Component);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
