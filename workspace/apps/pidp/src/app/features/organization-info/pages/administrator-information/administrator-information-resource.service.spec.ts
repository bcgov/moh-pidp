import { TestBed } from '@angular/core/testing';

import { AdministratorInformationResource } from './administrator-information-resource.service';

describe('AdministratorInformationResource', () => {
  let service: AdministratorInformationResource;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AdministratorInformationResource);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
