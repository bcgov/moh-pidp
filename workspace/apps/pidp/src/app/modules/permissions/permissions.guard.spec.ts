import { TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';

import { provideAutoSpy } from 'jest-auto-spies';

import { PermissionsGuard } from './permissions.guard';
import { PermissionsService } from './permissions.service';

describe('PermissionsGuard', () => {
  let guard: PermissionsGuard;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [RouterTestingModule],
      providers: [PermissionsGuard, provideAutoSpy(PermissionsService)],
    });

    guard = TestBed.inject(PermissionsGuard);
  });

  it('should be created', () => {
    expect(guard).toBeTruthy();
  });
});
