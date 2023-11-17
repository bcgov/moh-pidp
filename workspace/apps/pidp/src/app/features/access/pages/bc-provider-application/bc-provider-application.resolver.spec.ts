import { TestBed } from '@angular/core/testing';
import { provideAutoSpy } from 'jest-auto-spies';

import { bcProviderApplicationResolver } from './bc-provider-application.resolver';
import { PartyService } from '@app/core/party/party.service';
import { BcProviderApplicationResource } from './bc-provider-application-resource.service';

describe('BcProviderApplicationResolver', () => {
  let resolver: bcProviderApplicationResolver;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        provideAutoSpy(PartyService),
        provideAutoSpy(BcProviderApplicationResource),
      ],
    });
    resolver = TestBed.inject(bcProviderApplicationResolver);
  });

  it('should be created', () => {
    expect(resolver).toBeTruthy();
  });
});
