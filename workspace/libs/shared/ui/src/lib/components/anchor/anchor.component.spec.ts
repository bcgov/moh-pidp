import { TestBed } from '@angular/core/testing';

import { AnchorComponent } from './anchor.component';

describe('AnchorComponent', () => {
  let component: AnchorComponent;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [AnchorComponent],
    });

    component = TestBed.inject(AnchorComponent);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
