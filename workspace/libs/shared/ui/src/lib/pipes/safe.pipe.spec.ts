import { TestBed } from '@angular/core/testing';
import { DomSanitizer } from '@angular/platform-browser';

import { DomSanitizerType, SafePipe } from './safe.pipe';

describe('SafePipe', () => {
  let pipe: SafePipe;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [SafePipe],
    });
    pipe = TestBed.inject(SafePipe);
  });

  it('create an instance', () => expect(pipe).toBeTruthy());

  it('should not sanitize anything when value does not exist', () => {
    const value = null;
    const result = pipe.transform(value, 'html');
    expect(result).toBeNull();
  });

  it('should not throw an exception if the type of DOM sanitizer exists', () => {
    const value = 'Only testing that a DOM sanitizer exists';
    const domSanitizerTypes: DomSanitizerType[] = [
      'html',
      'style',
      'script',
      'url',
      'resourceUrl',
    ];
    let result = true;
    try {
      result = domSanitizerTypes.reduce(
        (finalResult, type) => finalResult && !!pipe.transform(value, type),
        result,
      );
    } catch {
      result = false;
    }
    expect(result).toBe(true);
  });

  it('should throw an exception when type of DOM sanitizer does not exist', () => {
    const value = 'Does not matter what this contains';
    const domSanitizerType = 'doesNotExist' as DomSanitizerType; // force type for testing
    const message = `Invalid safe type specified: ${domSanitizerType}`;
    let result = null;
    try {
      pipe.transform(value, domSanitizerType);
    } catch {
      result = message;
    }
    expect(result).toBe(message);
  });
});
