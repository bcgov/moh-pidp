import { TestBed } from '@angular/core/testing';

import { UserAccessAgreementResourceService } from './user-access-agreement-resource.service';

describe('UserAccessAgreementResourceService', () => {
  let service: UserAccessAgreementResourceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(UserAccessAgreementResourceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
