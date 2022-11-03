import { TestBed } from '@angular/core/testing';

import { MsTeamsPage } from './ms-teams.page';

describe('MsTeamsPage', () => {
  let component: MsTeamsPage;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [MsTeamsPage],
    });
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
