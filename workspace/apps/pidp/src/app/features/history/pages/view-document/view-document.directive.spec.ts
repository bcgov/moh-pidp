import { Component, TemplateRef, ViewContainerRef } from '@angular/core';
import { TestBed } from '@angular/core/testing';

import { provideAutoSpy } from 'jest-auto-spies';

import { ViewDocumentDirective } from './view-document.directive';

@Component({ selector: 'app-stub', template: '' })
class StubComponent {}

describe('ViewDocumentDirective', () => {
  let directive: ViewDocumentDirective;
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        ViewDocumentDirective,
        provideAutoSpy(StubComponent),
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
    directive = TestBed.inject(ViewDocumentDirective);
  });

  it('should create an instance', () => {
    expect(directive).toBeTruthy();
  });
});
