import { TestBed } from '@angular/core/testing';

import { UserAccessAgreementPage } from './user-access-agreement.page';

describe('UserAccessAgreementPage', () => {
  let component: UserAccessAgreementPage;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [UserAccessAgreementPage],
    });

    component = TestBed.inject(UserAccessAgreementPage);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
