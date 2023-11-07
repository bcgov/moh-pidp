import { TestBed } from '@angular/core/testing';

import { APP_CONFIG, APP_DI_CONFIG } from '@app/app.config';
import { HttpClient } from '@angular/common/http';
import { provideAutoSpy } from 'jest-auto-spies';
import { PartyActionsResolver } from './party-actions.resolver';

describe('PartyActionsResolver', () => {
  let resolver: PartyActionsResolver;

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
    resolver = TestBed.inject(PartyActionsResolver);
  });

  it('should be created', () => {
    expect(resolver).toBeTruthy();
  });
});
