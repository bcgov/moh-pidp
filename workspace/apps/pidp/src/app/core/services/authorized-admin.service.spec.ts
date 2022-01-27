import { TestBed } from '@angular/core/testing';

import { AuthorizedAdminService } from './authorized-admin.service';

describe('AuthorizedAdminService', () => {
  let service: AuthorizedAdminService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AuthorizedAdminService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
