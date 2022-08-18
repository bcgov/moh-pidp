import { TestBed } from '@angular/core/testing';

import { UciResource } from './uci-resource.service';

describe('UciResource', () => {
  let service: UciResource;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(UciResource);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
