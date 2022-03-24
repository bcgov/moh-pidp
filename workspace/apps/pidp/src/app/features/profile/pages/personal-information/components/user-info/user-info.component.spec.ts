import { TestBed } from '@angular/core/testing';

import { UserInfoComponent } from './user-info.component';

describe('UserInfoComponent', () => {
  let component: UserInfoComponent;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [UserInfoComponent],
    });
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
