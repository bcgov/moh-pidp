import { TestBed } from '@angular/core/testing';

import { ResourceUtilsService } from './resource-utils.service';

describe('ResourceUtilsService', () => {
  let service: ResourceUtilsService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ResourceUtilsService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
