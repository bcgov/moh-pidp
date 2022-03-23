import { TestBed } from '@angular/core/testing';

import { TermsOfAccessPage } from './terms-of-access.page';

describe('TermsOfAccessPage', () => {
  let component: TermsOfAccessPage;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [TermsOfAccessPage],
    });

    component = TestBed.inject(TermsOfAccessPage);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
