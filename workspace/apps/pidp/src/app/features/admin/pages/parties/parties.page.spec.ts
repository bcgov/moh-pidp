import { TestBed } from '@angular/core/testing';

import { PartiesPage } from './parties.page';

describe('PartiesComponent', () => {
  let component: PartiesPage;

  beforeEach(async () => {
    TestBed.configureTestingModule({
      providers: [PartiesPage],
    });

    component = TestBed.inject(PartiesPage);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
