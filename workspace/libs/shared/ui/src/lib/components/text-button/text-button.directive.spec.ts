import { ElementRef } from '@angular/core';

import { TextButtonDirective } from './text-button.directive';

describe('TextButtonDirective', () => {
  const el: ElementRef<HTMLButtonElement> = {} as ElementRef<HTMLButtonElement>;
  it('should create an instance', () => {
    const directive = new TextButtonDirective(el);
    expect(directive).toBeTruthy();
  });
});
