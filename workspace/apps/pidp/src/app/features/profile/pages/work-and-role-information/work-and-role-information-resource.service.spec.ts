import { TestBed } from '@angular/core/testing';

import { WorkAndRoleInformationResource } from './work-and-role-information-resource.service';

describe('WorkAndRoleInformationResource', () => {
  let service: WorkAndRoleInformationResource;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(WorkAndRoleInformationResource);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
