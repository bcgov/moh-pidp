import { TestBed } from '@angular/core/testing';

import { provideAutoSpy } from 'jest-auto-spies';

import { APP_CONFIG, APP_DI_CONFIG } from '@app/app.config';
import { AccessTokenService } from '@app/features/auth/services/access-token.service';
import { AuthService } from '@app/features/auth/services/auth.service';

import { AdminDashboardComponent } from './admin-dashboard.component';

describe('AdminDashboardComponent', () => {
  let component: AdminDashboardComponent;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        AdminDashboardComponent,
        {
          provide: APP_CONFIG,
          useValue: APP_DI_CONFIG,
        },
        provideAutoSpy(AuthService),
        provideAutoSpy(AccessTokenService),
      ],
    });

    // component = TestBed.inject(AdminDashboardComponent);
  });

  it('should create', () => {
    // expect(component).toBeTruthy();
  });
});
