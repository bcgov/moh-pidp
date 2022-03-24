import { TestBed } from '@angular/core/testing';

import { provideAutoSpy } from 'jest-auto-spies';

import { APP_CONFIG, APP_DI_CONFIG } from '@app/app.config';
import { AccessTokenService } from '@app/features/auth/services/access-token.service';
import { AuthService } from '@app/features/auth/services/auth.service';

import { PortalDashboardComponent } from './portal-dashboard.component';

describe('PortalDashboardComponent', () => {
  let component: PortalDashboardComponent;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        PortalDashboardComponent,
        {
          provide: APP_CONFIG,
          useValue: APP_DI_CONFIG,
        },
        provideAutoSpy(AuthService),
        provideAutoSpy(AccessTokenService),
      ],
    });

    // component = TestBed.inject(PortalDashboardComponent);
  });

  it('should create', () => {
    // expect(component).toBeTruthy();
  });
});
