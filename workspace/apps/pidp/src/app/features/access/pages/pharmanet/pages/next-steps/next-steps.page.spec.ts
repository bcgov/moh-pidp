import { TestBed } from '@angular/core/testing';

import { NextStepsPage } from './next-steps.page';

describe('NextStepsPage', () => {
  let component: NextStepsPage;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [NextStepsPage],
    });

    component = TestBed.inject(NextStepsPage);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
