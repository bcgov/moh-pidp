import { TestBed } from '@angular/core/testing';

import { MsTeamsResolver } from './ms-teams.resolver';

describe('MsTeamsResolver', () => {
  let resolver: MsTeamsResolver;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    resolver = TestBed.inject(MsTeamsResolver);
  });

  it('should be created', () => {
    expect(resolver).toBeTruthy();
  });
});
