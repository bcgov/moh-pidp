import { TestBed } from '@angular/core/testing';

import { PageFooterComponent } from './page-footer.component';

describe('PageFooterComponent', () => {
  let component: PageFooterComponent;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [PageFooterComponent],
    });

    component = TestBed.inject(PageFooterComponent);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
