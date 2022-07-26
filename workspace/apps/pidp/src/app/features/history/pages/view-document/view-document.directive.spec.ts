import { TemplateRef, ViewContainerRef } from '@angular/core';
import { TestBed } from '@angular/core/testing';

import { ViewDocumentDirective } from './view-document.directive';

describe('ViewDocumentDirective', () => {
  let directive: ViewDocumentDirective;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        ViewDocumentDirective,
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
