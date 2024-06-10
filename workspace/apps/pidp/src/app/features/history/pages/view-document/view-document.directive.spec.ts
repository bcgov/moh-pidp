import { Component } from '@angular/core';
import { TestBed } from '@angular/core/testing';

import { ViewDocumentDirective } from './view-document.directive';

@Component({ selector: 'app-stub', template: '' })
class StubComponent {}

describe('ViewDocumentDirective', () => {
  beforeEach(async () => {
    TestBed.configureTestingModule({
      declarations: [StubComponent],
    }).compileComponents();
  });

  it('should create an instance', () => {
    const fixture = TestBed.createComponent(StubComponent);
    const component = fixture.debugElement.componentInstance;

    const directive = new ViewDocumentDirective(component);
    expect(directive).toBeTruthy();
  });
});
