/* eslint-disable @typescript-eslint/no-explicit-any */
import { HttpStatusCode } from '@angular/common/http';
import { TestBed } from '@angular/core/testing';

import { randNumber } from '@ngneat/falso';
import { Spy, createSpyFromClass, provideAutoSpy } from 'jest-auto-spies';

import { PartyService } from '@app/core/party/party.service';

import { SaEformsResource } from './sa-eforms-resource.service';
import { SaEformsResolver } from './sa-eforms.resolver';

describe('SaEformsResolver', () => {
  let resolver: SaEformsResolver;
  let saEformsResourceSpy: Spy<SaEformsResource>;
  let partyServiceSpy: Spy<PartyService>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        SaEformsResolver,
        provideAutoSpy(SaEformsResource),
        {
          provide: PartyService,
          useValue: createSpyFromClass(PartyService, {
            gettersToSpyOn: ['partyId'],
            settersToSpyOn: ['partyId'],
          }),
        },
      ],
    });

    resolver = TestBed.inject(SaEformsResolver);
    saEformsResourceSpy = TestBed.inject<any>(SaEformsResource);
    partyServiceSpy = TestBed.inject<any>(PartyService);
  });

  describe('METHOD: resolve', () => {
    given('partyId exists', () => {
      const partyId = randNumber({ min: 1 });
      partyServiceSpy.accessorSpies.getters.partyId.mockReturnValue(partyId);

      when('attempting to resolve routing and request successful', () => {
        saEformsResourceSpy.requestAccess
          .mustBeCalledWith(partyId)
          .nextOneTimeWith(void 0);
        let actualResult: boolean;
        resolver
          .resolve()
          .subscribe((result: boolean) => (actualResult = result));

        then('will allow routing', () => {
          expect(saEformsResourceSpy.requestAccess).toHaveBeenCalledTimes(1);
          expect(actualResult).toBe(true);
        });
      });
    });

    given('partyId exists', () => {
      const partyId = randNumber({ min: 1 });
      partyServiceSpy.accessorSpies.getters.partyId.mockReturnValue(partyId);
      when('attempting to resolve routing and request unsuccessful', () => {
        saEformsResourceSpy.requestAccess
          .mustBeCalledWith(partyId)
          .nextWithValues([
            {
              errorValue: {
                status: HttpStatusCode.NotFound,
              },
            },
          ]);
        let actualResult: boolean;
        resolver
          .resolve()
          .subscribe((result: boolean) => (actualResult = result));

        then('will prevent routing to the component', () => {
          expect(saEformsResourceSpy.requestAccess).toHaveBeenCalledTimes(1);
          expect(actualResult).toBe(false);
        });
      });
    });

    given('partyId does not exists', () => {
      const partyId = null;
      partyServiceSpy.accessorSpies.getters.partyId.mockReturnValue(partyId);

      when('attempting to resolve routing', () => {
        let actualResult: boolean;
        resolver
          .resolve()
          .subscribe((result: boolean) => (actualResult = result));

        then('will prevent routing to the component', () => {
          expect(saEformsResourceSpy.requestAccess).not.toHaveBeenCalled();
          expect(actualResult).toBe(false);
        });
      });
    });
  });
});
