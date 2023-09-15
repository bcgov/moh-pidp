import { ElementRef } from '@angular/core';
import { TestBed } from '@angular/core/testing';

import { provideAutoSpy } from 'jest-auto-spies';

import { PhonePipe } from '../../pipes';
import { AnchorDirective } from './anchor.directive';

describe('AnchorDirective', () => {
  let directive: AnchorDirective;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        AnchorDirective,
        provideAutoSpy(ElementRef),
        provideAutoSpy(PhonePipe),
      ],
    });

    directive = TestBed.inject(AnchorDirective);
  });

  it('should create an instance', () => {
    expect(directive).toBeTruthy();
  });
});
