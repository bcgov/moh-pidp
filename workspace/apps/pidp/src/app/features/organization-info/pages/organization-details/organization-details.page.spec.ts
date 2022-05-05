import { TestBed } from '@angular/core/testing';

import { OrganizationDetailsPage } from './organization-details.page';

describe('OrganizationDetailsPage', () => {
  let component: OrganizationDetailsPage;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [OrganizationDetailsPage],
    });

    component = TestBed.inject(OrganizationDetailsPage);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
