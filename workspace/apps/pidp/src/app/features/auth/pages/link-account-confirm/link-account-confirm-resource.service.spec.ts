import { TestBed } from '@angular/core/testing';

import { LinkAccountConfirmResourceService } from './link-account-confirm-resource.service';

describe('LinkAccountConfirmResourceService', () => {
  let service: LinkAccountConfirmResourceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(LinkAccountConfirmResourceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
