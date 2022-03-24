import { TemplateRef, ViewContainerRef } from '@angular/core';
import { TestBed } from '@angular/core/testing';

import { provideAutoSpy } from 'jest-auto-spies';

import { PermissionsDirective } from './permissions.directive';
import { PermissionsService } from './permissions.service';

describe('PermissionsDirective', () => {
  let directive: PermissionsDirective;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        PermissionsDirective,
        provideAutoSpy(PermissionsService),
        {
          provide: ViewContainerRef,
          useValue: {},
        },
        {
          provide: TemplateRef,
          useValue: {},
        },
      ],
    });

    directive = TestBed.inject(PermissionsDirective);
  });

  it('should create an instance', () => {
    expect(directive).toBeTruthy();
  });
});
