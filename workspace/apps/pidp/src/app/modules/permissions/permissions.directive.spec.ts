import { TestBed } from '@angular/core/testing';

import { PermissionsDirective } from './permissions.directive';

describe('PermissionsDirective', () => {
  let directive: PermissionsDirective;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [PermissionsDirective],
    });

    directive = TestBed.inject(PermissionsDirective);
  });

  it('should create an instance', () => {
    expect(directive).toBeTruthy();
  });
});
