import { TestBed } from '@angular/core/testing';

import { ApiResourceUtilsService } from './api-resource-utils.service';

describe('ApiResourceUtilsService', () => {
  let service: ApiResourceUtilsService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ApiResourceUtilsService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
