import { TestBed } from '@angular/core/testing';

import { OrganizationDetailsComponent } from './organization-details.component';

describe('OrganizationDetailsComponent', () => {
  let component: OrganizationDetailsComponent;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [OrganizationDetailsComponent],
    });
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
