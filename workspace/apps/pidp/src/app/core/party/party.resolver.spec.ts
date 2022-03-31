/* eslint-disable @typescript-eslint/no-explicit-any */
import { TestBed } from '@angular/core/testing';
import { Router } from '@angular/router';
import { RouterTestingModule } from '@angular/router/testing';

import { randNumber } from '@ngneat/falso';
import { Spy, createSpyFromClass, provideAutoSpy } from 'jest-auto-spies';

import { RootRoutes } from '@bcgov/shared/ui';

import { APP_CONFIG, APP_DI_CONFIG } from '@app/app.config';

import { DocumentService } from '../services/document.service';
import { LoggerService } from '../services/logger.service';
import { PartyResource } from './party-resource.service';
import { PartyResolver } from './party.resolver';
import { PartyService } from './party.service';

describe('PartyResolver', () => {
  let resolver: PartyResolver;
  let partyResourceSpy: Spy<PartyResource>;
  let partyServiceSpy: Spy<PartyService>;
  let router: Router;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [RouterTestingModule],
      providers: [
        PartyResolver,
        {
          provide: APP_CONFIG,
          useValue: APP_DI_CONFIG,
        },
        {
          provide: PartyService,
          useValue: createSpyFromClass(PartyService, {
            gettersToSpyOn: ['partyId'],
            settersToSpyOn: ['partyId'],
          }),
        },
        provideAutoSpy(PartyResource),
        provideAutoSpy(LoggerService),
        provideAutoSpy(DocumentService),
        provideAutoSpy(Router),
      ],
    });

    router = TestBed.inject(Router);
    resolver = TestBed.inject(PartyResolver);
    partyResourceSpy = TestBed.inject<any>(PartyResource);
    partyServiceSpy = TestBed.inject<any>(PartyService);
  });

  describe('METHOD: resolve', () => {
    given('a party ID does not exist', () => {
      partyServiceSpy.accessorSpies.setters.partyId(null);

      when('attempting to resolve the party is successful', () => {
        const partyId = randNumber();
        partyResourceSpy.firstOrCreate.nextOneTimeWith(partyId);
        let actualResult: number | null;
        resolver
          .resolve()
          .subscribe((partyId: number | null) => (actualResult = partyId));

        then('response will provide the party ID', () => {
          expect(partyResourceSpy.firstOrCreate).toHaveBeenCalledTimes(1);
          expect(actualResult).toBe(partyId);
        });
      });
    });

    given('a party ID does not exist', () => {
      partyServiceSpy.accessorSpies.setters.partyId(null);

      when('attempting to resolve the party is unsuccessful', () => {
        partyResourceSpy.firstOrCreate.nextWithValues([
          {
            errorValue: new Error('Anonymous error of any type'),
          },
        ]);
        let actualResult: number | null;
        resolver
          .resolve()
          .subscribe((partyId: number | null) => (actualResult = partyId));

        then('response will provide the party ID', () => {
          expect(partyResourceSpy.firstOrCreate).toHaveBeenCalledTimes(1);
          expect(router.navigateByUrl).toHaveBeenCalledWith(RootRoutes.DENIED);
          expect(actualResult).toBe(null);
        });
      });
    });
  });
});
