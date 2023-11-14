import { TestBed } from '@angular/core/testing';
import { MatDialogModule } from '@angular/material/dialog';
import { ActivatedRoute } from '@angular/router';
import { RouterTestingModule } from '@angular/router/testing';

import { randTextRange } from '@ngneat/falso';
import { provideAutoSpy } from 'jest-auto-spies';

import { APP_CONFIG, APP_DI_CONFIG } from '@app/app.config';
import { DocumentService } from '@app/core/services/document.service';

import { AuthService } from '../../services/auth.service';
import { LoginPage, LoginPageRouteData } from './login.page';
import { ClientLogsService } from '@app/core/services/client-logs.service';

describe('LoginPage', () => {
  let component: LoginPage;

  let mockActivatedRoute;

  beforeEach(() => {
    mockActivatedRoute = {
      snapshot: {
        queryParams: { action: '' },
        data: {
          loginPageData: {
            title: randTextRange({ min: 1, max: 4 }),
            isAdminLogin: true,
          } as LoginPageRouteData,
        },
      },
    };

    TestBed.configureTestingModule({
      imports: [RouterTestingModule, MatDialogModule],
      providers: [
        LoginPage,
        {
          provide: APP_CONFIG,
          useValue: APP_DI_CONFIG,
        },
        {
          provide: ActivatedRoute,
          useValue: mockActivatedRoute,
        },
        provideAutoSpy(AuthService),
        provideAutoSpy(ClientLogsService),
        provideAutoSpy(DocumentService),
      ],
    });

    component = TestBed.inject(LoginPage);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
