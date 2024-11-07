import { TestBed } from '@angular/core/testing';

import { APP_CONFIG, APP_DI_CONFIG } from '@app/app.config';

import { SnowplowService } from './snowplow.service';
import { WindowRefService } from './window-ref.service';

describe('SnowplowService', () => {
  let service: SnowplowService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        WindowRefService,
        {
          provide: APP_CONFIG,
          useValue: APP_DI_CONFIG,
        },
      ],
    });
    service = TestBed.inject(SnowplowService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
