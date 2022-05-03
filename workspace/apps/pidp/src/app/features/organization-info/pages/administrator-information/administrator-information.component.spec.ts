import { TestBed } from '@angular/core/testing';

import { AdministratorInformationComponent } from './administrator-information.component';

describe('AdministratorInformationComponent', () => {
  let component: AdministratorInformationComponent;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [AdministratorInformationComponent],
    });

    component = TestBed.inject(AdministratorInformationComponent);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
