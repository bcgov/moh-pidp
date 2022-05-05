import { TestBed } from '@angular/core/testing';

import { FacilityDetailsPage } from './facility-details.page';

describe('FacilityDetailsPage', () => {
  let component: FacilityDetailsPage;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [FacilityDetailsPage],
    });

    component = TestBed.inject(FacilityDetailsPage);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
