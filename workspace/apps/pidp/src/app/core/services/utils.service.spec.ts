import { TestBed } from '@angular/core/testing';

import { APP_CONFIG, APP_DI_CONFIG } from '../../app.config';
import { UtilsService } from './utils.service';

describe('UtilsService', () => {
  beforeEach(() =>
    TestBed.configureTestingModule({
      providers: [
        {
          provide: APP_CONFIG,
          useValue: APP_DI_CONFIG,
        },
        {
          provide: Window,
          useValue: {},
        },
        {
          provide: Document,
          useValue: {},
        },
      ],
    }),
  );

  it('should create', () => {
    const service: UtilsService = TestBed.inject(UtilsService);
    expect(service).toBeTruthy();
  });
});
