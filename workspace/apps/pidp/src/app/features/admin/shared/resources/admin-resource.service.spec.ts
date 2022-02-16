import { TestBed } from '@angular/core/testing';

import { AdminResource } from './admin-resource.service';

describe('AdminResource', () => {
  let service: AdminResource;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AdminResource);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
