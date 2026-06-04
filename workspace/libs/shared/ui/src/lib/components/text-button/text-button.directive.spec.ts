import { TestBed } from '@angular/core/testing';

import { TextButtonDirective } from './text-button.directive';

describe('TextButtonDirective', () => {
  it('should create an instance', () => {
    TestBed.configureTestingModule({
      providers: [TextButtonDirective],
    });
    const directive = TestBed.inject(TextButtonDirective);
    expect(directive).toBeTruthy();
  });
});
