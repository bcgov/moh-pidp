import { TestBed } from '@angular/core/testing';

import { LandingActionsResolver } from './landing-actions.resolver';
import { APP_CONFIG, APP_DI_CONFIG } from '@app/app.config';
import { HttpClient } from '@angular/common/http';
import { provideAutoSpy } from 'jest-auto-spies';

describe('LandingActionsResolver', () => {
  let resolver: LandingActionsResolver;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        {
          provide: APP_CONFIG,
          useValue: APP_DI_CONFIG,
        },
        provideAutoSpy(HttpClient),
      ],
    });
    resolver = TestBed.inject(LandingActionsResolver);
  });

  it('should be created', () => {
    expect(resolver).toBeTruthy();
  });
});
