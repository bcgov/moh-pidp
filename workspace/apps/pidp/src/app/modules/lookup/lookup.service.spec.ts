/* eslint-disable @typescript-eslint/no-explicit-any */
import { TestBed } from '@angular/core/testing';

import { randSentence, randText, randWord } from '@ngneat/falso';
import { Spy, provideAutoSpy } from 'jest-auto-spies';

import { SortUtils } from '@bcgov/shared/utils';

import { UtilsService } from '@app/core/services/utils.service';

import { LookupResource } from './lookup-resource.service';
import { LookupService } from './lookup.service';
import { LookupConfig, lookupConfigKeys } from './lookup.types';

describe('LookupService', () => {
  let lookupService: LookupService;
  let lookupResourceSpy: Spy<LookupResource>;
  let mockLookupConfig: LookupConfig;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        LookupService,
        provideAutoSpy(LookupResource),
        provideAutoSpy(UtilsService),
      ],
    });

    lookupService = TestBed.inject(LookupService);
    lookupResourceSpy = TestBed.inject<any>(LookupResource);

    mockLookupConfig = getMockLookupConfig();
  });

  describe('METHOD load', () => {
    given('the lookups have not been requested', () => {
      lookupConfigKeys.forEach((key) =>
        expect(lookupService[key].length).toEqual(0),
      );
      lookupResourceSpy.getLookups.nextOneTimeWith(mockLookupConfig);

      when('the lookups are loaded', () => {
        const lookups$ = lookupService.load();

        then('they should be loaded through an HTTP request', () => {
          expect(lookupResourceSpy.getLookups).toHaveBeenCalledTimes(1);
          lookups$.subscribe((lookups) =>
            expect(lookups).toEqual(mockLookupConfig),
          );
          lookupConfigKeys.forEach((key) =>
            expect(lookupService[key].length).toBeGreaterThan(1),
          );
        });
      });
    });

    given('the lookups have been requested', () => {
      lookupResourceSpy.getLookups.nextOneTimeWith(mockLookupConfig);
      let lookups$ = lookupService.load();
      lookups$
        .subscribe((lookups) => expect(lookups).toEqual(mockLookupConfig))
        .unsubscribe();

      when('the lookups are reloaded another request is not made', () => {
        lookups$ = lookupService.load();

        then('they should be loaded using the service', () => {
          expect(lookupResourceSpy.getLookups).toHaveBeenCalledTimes(1);
          lookups$.subscribe((lookups) =>
            expect(lookups).toEqual(mockLookupConfig),
          );
        });
      });
    });
  });

  describe('METHOD colleges', () => {
    given('the lookup exists and is not sorted by code', () => {
      const collegesSortedByName = [...mockLookupConfig.colleges].sort(
        SortUtils.sortByKey('name'),
      );
      const mockLookupConfigSortedByName = { ...mockLookupConfig };
      mockLookupConfigSortedByName.colleges = collegesSortedByName;
      lookupResourceSpy.getLookups.nextOneTimeWith(
        mockLookupConfigSortedByName,
      );

      when('the lookup is accessed', () => {
        lookupService.load().subscribe();

        then('it should be copied and sorted by code', () => {
          expect(lookupService.colleges).toEqual(mockLookupConfig.colleges);
        });
      });
    });
  });
});

/**
 * @description
 * Mock generator for the LookupConfig.
 */
function getMockLookupConfig(): LookupConfig {
  const array = Array(5)
    .fill(null)
    .map((_, code) => code);

  return {
    accessTypes: [...array].map((code) => ({
      code,
      name: randWord(),
    })),
    colleges: [...array].map((code) => ({
      code,
      name: randSentence(),
      acronym: randWord(),
    })),
    countries: [...array].map((_) => ({
      code: randText({ charCount: 2 }),
      name: randWord(),
    })),
    provinces: [...array].map((_) => ({
      code: randText({ charCount: 2 }),
      countryCode: randText({ charCount: 2 }),
      name: randWord(),
    })),
    organizations: [...array].map((code) => ({
      code,
      name: randWord(),
    })),
    healthAuthorities: [...array].map((code) => ({
      code,
      name: randWord(),
    })),
  };
}
