/* eslint-disable @typescript-eslint/no-explicit-any */
import { TestBed } from '@angular/core/testing';
import { ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';

import { Spy, provideAutoSpy } from 'jest-auto-spies';

import { AbstractFormState } from '@bcgov/shared/ui';

import { IFormPage } from '../classes/abstract-form-page.class';
import { canDeactivateFormGuard } from './can-deactivate-form.guard';

describe('canDeactivateFormGuard', () => {
  let activatedRouteSnapshotSpy: Spy<ActivatedRouteSnapshot>;
  let routerStateSnapshotSpy: Spy<RouterStateSnapshot>;
  let component: IFormPage;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        provideAutoSpy(ActivatedRouteSnapshot),
        provideAutoSpy(RouterStateSnapshot),
      ],
    });

    activatedRouteSnapshotSpy = TestBed.inject<any>(ActivatedRouteSnapshot);
    routerStateSnapshotSpy = TestBed.inject<any>(RouterStateSnapshot);

    component = {
      canDeactivate: (): boolean => {
        return true;
      },
      formState: {} as AbstractFormState<unknown>,
      onSubmit: (): void => {
        return;
      },
    };
  });
  describe('METHOD: canDeactivate', () => {
    given('canDeactivate component fn return true', () => {
      component.canDeactivate = (): boolean => true;

      when('the guard is called', () => {
        const result = TestBed.runInInjectionContext(() =>
          canDeactivateFormGuard(
            component,
            activatedRouteSnapshotSpy,
            routerStateSnapshotSpy,
            routerStateSnapshotSpy,
          ),
        );

        then('The form should be deactivate', () => {
          expect(result).toBeTruthy();
        });
      });
    });
  });

  given('canDeactivate component fn return false', () => {
    component.canDeactivate = (): boolean => false;

    when('the guard is called', () => {
      const result = TestBed.runInInjectionContext(() =>
        canDeactivateFormGuard(
          component,
          activatedRouteSnapshotSpy,
          routerStateSnapshotSpy,
          routerStateSnapshotSpy,
        ),
      );

      then('The form should not be deactivate', () => {
        expect(result).toBeFalsy();
      });
    });
  });

  given('canDeactivate component fn is undefined', () => {
    component.canDeactivate = undefined as any;

    when('the guard is called', () => {
      const result = TestBed.runInInjectionContext(() =>
        canDeactivateFormGuard(
          component,
          activatedRouteSnapshotSpy,
          routerStateSnapshotSpy,
          routerStateSnapshotSpy,
        ),
      );

      then('The form should be deactivate', () => {
        expect(result).toBeTruthy();
      });
    });
  });
});
