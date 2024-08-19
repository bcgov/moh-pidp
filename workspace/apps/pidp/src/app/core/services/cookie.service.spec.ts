import { TestBed } from '@angular/core/testing';


import { CryptoService } from './crypto.service';
import { APP_CONFIG, APP_DI_CONFIG } from '@app/app.config';
import { createSpyFromClass } from 'jest-auto-spies';

describe('CryptoService', () => {
  let service: CryptoService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        {
          provide: APP_CONFIG,
          useValue: APP_DI_CONFIG,
        },
        {
          provide: CryptoService,
          useValue: createSpyFromClass(CryptoService),
        },
      ]
    });

    service = TestBed.inject(CryptoService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
