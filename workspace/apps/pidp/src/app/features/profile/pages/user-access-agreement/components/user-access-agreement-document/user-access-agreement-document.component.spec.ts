import { TestBed } from '@angular/core/testing';

import { UserAccessAgreementDocumentComponent } from './user-access-agreement-document.component';

describe('UserAccessAgreementDocumentComponent', () => {
  let component: UserAccessAgreementDocumentComponent;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [UserAccessAgreementDocumentComponent],
    });

    component = TestBed.inject(UserAccessAgreementDocumentComponent);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
