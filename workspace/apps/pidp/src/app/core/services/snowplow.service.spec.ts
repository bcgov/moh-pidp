import { TestBed } from '@angular/core/testing';

import { provideAutoSpy } from 'jest-auto-spies';

import { APP_CONFIG, APP_DI_CONFIG } from '@app/app.config';

import { SnowplowService } from './snowplow.service';
import { WindowRefService } from './window-ref.service';

describe('SnowplowService', () => {
  let service: SnowplowService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        SnowplowService,
        {
          provide: APP_CONFIG,
          useValue: APP_DI_CONFIG,
        },
        provideAutoSpy(WindowRefService),
      ],
    });
    service = TestBed.inject(SnowplowService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
