import { TestBed } from '@angular/core/testing';

import { SnowplowService } from './snowplow.service';

describe('SnowplowService', () => {
  let service: SnowplowService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SnowplowService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
