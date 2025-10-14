import { Component, ViewContainerRef } from '@angular/core';
import { TestBed } from '@angular/core/testing';

import { ViewDocumentDirective } from './view-document.directive';

@Component({ selector: 'app-stub', template: '' })
class StubComponent {}

describe('ViewDocumentDirective', () => {
  let fixture;
  let component: ViewContainerRef;
  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [StubComponent],
    });
    fixture = TestBed.createComponent(StubComponent);
    component = fixture.debugElement.componentInstance;
    fixture.detectChanges();
  });

  it('should create an instance', () => {
    const directive = new ViewDocumentDirective(component);
    expect(directive).toBeTruthy();
  });
});
