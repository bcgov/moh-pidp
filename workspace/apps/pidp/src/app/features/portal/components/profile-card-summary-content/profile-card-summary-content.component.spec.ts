import { TestBed } from '@angular/core/testing';

import { ProfileCardSummaryContentComponent } from './profile-card-summary-content.component';

describe('ProfileCardSummaryContentComponent', () => {
  let component: ProfileCardSummaryContentComponent;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [ProfileCardSummaryContentComponent],
    });

    component = TestBed.inject(ProfileCardSummaryContentComponent);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
