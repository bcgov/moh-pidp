import { TestBed } from '@angular/core/testing';

import * as CryptoJS from 'crypto-js';

import { CryptoService } from './crypto.service';

describe('CryptoService', () => {
  let service: CryptoService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [CryptoJS],
      providers: [CryptoService],
    });

    service = TestBed.inject(CryptoService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
