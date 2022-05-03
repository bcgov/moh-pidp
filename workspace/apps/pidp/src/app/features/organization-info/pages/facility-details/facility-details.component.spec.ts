import { TestBed } from '@angular/core/testing';

import { FacilityDetailsComponent } from './facility-details.component';

describe('FacilityDetailsComponent', () => {
  let component: FacilityDetailsComponent;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [FacilityDetailsComponent],
    });

    component = TestBed.inject(FacilityDetailsComponent);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
