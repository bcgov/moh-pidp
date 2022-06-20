import { HttpClientTestingModule } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';

import { APP_CONFIG, APP_DI_CONFIG } from '@app/app.config';

import { OrganizationDetailsResource } from './organization-details-resource.service';

describe('OrganizationDetailsResource', () => {
  let service: OrganizationDetailsResource;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [
        OrganizationDetailsResource,
        {
          provide: APP_CONFIG,
          useValue: APP_DI_CONFIG,
        },
      ],
    });

    service = TestBed.inject(OrganizationDetailsResource);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
