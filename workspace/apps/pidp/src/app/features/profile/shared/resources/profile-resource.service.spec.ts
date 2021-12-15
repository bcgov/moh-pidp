import { TestBed } from '@angular/core/testing';

import { ProfileResourceService } from './profile-resource.service';

describe('ProfileResourceService', () => {
  let service: ProfileResourceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ProfileResourceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
