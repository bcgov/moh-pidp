import { TestBed } from '@angular/core/testing';

import { SaEformsPage } from './sa-eforms.page';

describe('SaEformsPage', () => {
  let component: SaEformsPage;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [SaEformsPage],
    });

    component = TestBed.inject(SaEformsPage);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
