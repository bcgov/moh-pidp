import { TestBed } from '@angular/core/testing';
import { CanDeactivateFn } from '@angular/router';

import { IFormPage } from '../classes/abstract-form-page.class';
import { canDeactivateFormGuard } from './can-deactivate-form.guard';

describe('canDeactivateFormGuard', () => {
  const executeGuard: CanDeactivateFn<IFormPage> = (...guardParameters) =>
    TestBed.runInInjectionContext(() =>
      canDeactivateFormGuard(...guardParameters)
    );

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeGuard).toBeTruthy();
  });
});
