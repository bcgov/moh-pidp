/* eslint-disable @typescript-eslint/no-explicit-any */
import { TestBed } from '@angular/core/testing';

import { randNumber } from '@ngneat/falso';
import { MockLookup } from '@test/mock-lookup';
import { Spy, createSpyFromClass } from 'jest-auto-spies';

import { LookupCodePipe } from './lookup-code.pipe';
import { LookupService } from './lookup.service';

describe('LookupCodePipe', () => {
  let pipe: LookupCodePipe;
  let lookupService: Spy<LookupService>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        LookupCodePipe,
        {
          provide: LookupService,
          useValue: createSpyFromClass(LookupService, {
            gettersToSpyOn: ['countries', 'provinces'],
            settersToSpyOn: ['countries', 'provinces'],
          }),
        },
      ],
    });

    pipe = TestBed.inject(LookupCodePipe);
    lookupService = TestBed.inject<any>(LookupService);
  });

  describe('METHOD: transform', () => {
    given('a valid country code value', () => {
      const countries = MockLookup.get().countries;
      const country = countries[randNumber({ max: countries.length - 1 })];
      lookupService.accessorSpies.getters.countries.mockReturnValue(countries);

      when('tranforming the country code to the default name property', () => {
        const actualResult = pipe.transform(country.code, 'countries');

        then('a country name should be returned', () => {
          expect(actualResult).toBe(country.name);
        });
      });
    });

    given('a valid province code value', () => {
      const provinces = MockLookup.get().provinces;
      const province = provinces[randNumber({ max: provinces.length - 1 })];
      lookupService.accessorSpies.getters.provinces.mockReturnValue(provinces);

      when('tranforming the province code to a country code property', () => {
        const actualResult = pipe.transform(
          province.code,
          'provinces',
          'countryCode',
        );

        then('a province country code should be returned', () => {
          expect(actualResult).toBe(province.countryCode);
        });
      });
    });

    given('an invalid country code value of null', () => {
      const countries = MockLookup.get().countries;
      const value = '';
      lookupService.accessorSpies.getters.countries.mockReturnValue(countries);

      when('tranforming the null value', () => {
        const actualResult = pipe.transform(value, 'countries');

        then('an empty string should be returned', () => {
          expect(actualResult).toBe(null);
        });
      });
    });
  });
});
