import { TestBed } from '@angular/core/testing';
import { RouterModule } from '@angular/router';

import { provideAutoSpy } from 'jest-auto-spies';

import { APP_CONFIG, APP_DI_CONFIG } from '@app/app.config';

import { AuthService } from '../../services/auth.service';
import { BcProviderUpliftPage } from './bc-provider-uplift.page';

describe('BcProviderUpliftPage', () => {
  let component: BcProviderUpliftPage;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [RouterModule.forRoot([])],
      providers: [
        BcProviderUpliftPage,
        {
          provide: APP_CONFIG,
          useValue: APP_DI_CONFIG,
        },
        provideAutoSpy(AuthService),
      ],
    });

    component = TestBed.inject(BcProviderUpliftPage);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
