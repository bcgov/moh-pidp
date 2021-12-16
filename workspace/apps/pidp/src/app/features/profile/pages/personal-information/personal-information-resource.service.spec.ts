import { TestBed } from '@angular/core/testing';

import { PersonalInformationResource } from './personal-information-resource.service';

describe('ProfileInformationResource', () => {
  let service: PersonalInformationResource;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(PersonalInformationResource);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
