/* eslint-disable @typescript-eslint/no-explicit-any */
import { TestBed } from '@angular/core/testing';
import {
  ActivatedRouteSnapshot,
  ResolveFn,
  Router,
  RouterStateSnapshot,
} from '@angular/router';
import { RouterTestingModule } from '@angular/router/testing';

import { Observable } from 'rxjs';

import { randNumber } from '@ngneat/falso';
import { Spy, createSpyFromClass, provideAutoSpy } from 'jest-auto-spies';

import { APP_CONFIG, APP_DI_CONFIG } from '@app/app.config';
import { ShellRoutes } from '@app/features/shell/shell.routes';

import { DocumentService } from '../services/document.service';
import { LoggerService } from '../services/logger.service';
import { DiscoveryResource } from './discovery-resource.service';
import { partyResolver } from './party.resolver';
import { PartyService } from './party.service';

describe('partyResolver', () => {
  const executeResolver: ResolveFn<number | null> = (...resolverParameters) =>
    TestBed.runInInjectionContext(() => partyResolver(...resolverParameters));

  let partyResourceSpy: Spy<DiscoveryResource>;
  let partyServiceSpy: Spy<PartyService>;
  let router: Router;
  let activatedRouteSnapshotSpy: Spy<ActivatedRouteSnapshot>;
  let routerStateSnapshotSpy: Spy<RouterStateSnapshot>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [RouterTestingModule],
      providers: [
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
        provideAutoSpy(DiscoveryResource),
        provideAutoSpy(LoggerService),
        provideAutoSpy(DocumentService),
        provideAutoSpy(Router),
      ],
    });

    router = TestBed.inject(Router);
    partyResourceSpy = TestBed.inject<any>(DiscoveryResource);
    partyServiceSpy = TestBed.inject<any>(PartyService);
  });

  describe('METHOD: resolve', () => {
    given('a party ID does not exist', () => {
      partyServiceSpy.accessorSpies.setters.partyId(null);

      when('attempting to resolve the party is successful', () => {
        const discoveryResult = {
          partyId: randNumber(),
          newBCProvider: false,
        };
        partyResourceSpy.discover.nextOneTimeWith(discoveryResult);
        let actualResult: number | null;
        (
          executeResolver(
            activatedRouteSnapshotSpy,
            routerStateSnapshotSpy,
          ) as Observable<number | null>
        ).subscribe((partyId: number | null) => (actualResult = partyId));

        then('response will provide the party ID', () => {
          expect(partyResourceSpy.discover).toHaveBeenCalledTimes(1);
          expect(actualResult).toBe(discoveryResult.partyId);
        });
      });
    });

    given('a party ID does not exist', () => {
      partyServiceSpy.accessorSpies.setters.partyId(null);

      when('attempting to resolve the party is unsuccessful', () => {
        partyResourceSpy.discover.nextWithValues([
          {
            errorValue: new Error('Anonymous error of any type'),
          },
        ]);
        let actualResult: number | null;
        (
          executeResolver(
            activatedRouteSnapshotSpy,
            routerStateSnapshotSpy,
          ) as Observable<number | null>
        ).subscribe((partyId: number | null) => (actualResult = partyId));

        then('response will provide the party ID', () => {
          expect(partyResourceSpy.discover).toHaveBeenCalledTimes(1);
          expect(router.navigateByUrl).toHaveBeenCalledWith(
            ShellRoutes.SUPPORT_ERROR_PAGE,
          );
          expect(actualResult).toBe(null);
        });
      });
    });
  });
});
