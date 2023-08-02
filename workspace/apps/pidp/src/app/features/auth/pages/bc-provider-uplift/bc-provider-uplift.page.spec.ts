import { ComponentFixture, TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';

import { provideAutoSpy } from 'jest-auto-spies';
import { CookieService } from 'ngx-cookie-service';

import { APP_CONFIG, APP_DI_CONFIG } from '@app/app.config';

import { AuthService } from '../../services/auth.service';
import { BcProviderUpliftPage } from './bc-provider-uplift.page';

describe('BcProviderUpliftPage', () => {
  let component: BcProviderUpliftPage;
  let fixture: ComponentFixture<BcProviderUpliftPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RouterTestingModule],
      declarations: [BcProviderUpliftPage],
      providers: [
        provideAutoSpy(CookieService),
        {
          provide: APP_CONFIG,
          useValue: APP_DI_CONFIG,
        },
        provideAutoSpy(AuthService),
      ],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(BcProviderUpliftPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
