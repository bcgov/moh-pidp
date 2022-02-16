import { TestBed } from '@angular/core/testing';

import { MockLookupService } from '@test/mock-lookup.service';

import { LookupCodePipe } from './lookup-code.pipe';
import { LookupService } from './lookup.service';

describe('LookupCodePipe', () => {
  let pipe: LookupCodePipe;
  let lookupService: LookupService;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      providers: [
        {
          provide: LookupService,
          userClass: MockLookupService,
        },
      ],
    }).compileComponents();

    lookupService = TestBed.inject(LookupService);
    pipe = new LookupCodePipe(lookupService);
  });

  it('create an instance', () => expect(pipe).toBeTruthy());

  it('should get a config name from a config code', () => {
    const country = lookupService.countries[0];
    const result = pipe.transform(country.code, 'countries');
    expect(result).toBe(country.name);
  });

  it('should not fail when passed a null', () => {
    const result = pipe.transform(null, 'countries');
    expect(result).toBe('');
  });
});
