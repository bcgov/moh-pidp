import { TestBed } from '@angular/core/testing';

import { AddressInfoComponent } from './address-info.component';

describe('AddressInfoComponent', () => {
  let component: AddressInfoComponent;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [AddressInfoComponent],
    });

    component = TestBed.inject(AddressInfoComponent);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
