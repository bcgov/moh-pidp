import { TestBed } from '@angular/core/testing';

import { BusyService } from './busy.service';

describe('BusyService', () => {
  let service: BusyService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(BusyService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
