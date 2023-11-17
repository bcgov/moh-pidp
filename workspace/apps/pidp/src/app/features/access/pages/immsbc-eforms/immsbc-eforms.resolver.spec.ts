import { TestBed } from '@angular/core/testing';

import { provideAutoSpy } from 'jest-auto-spies';

import { PartyService } from '@app/core/party/party.service';

import { ImmsBCEformsResource } from './immsbc-eforms-resource.service';
import { immsBCEformsResolver } from './immsbc-eforms.resolver';

describe('ImmsBCEformsResolver', () => {
  let resolver: ImmsBCEformsResolver;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        provideAutoSpy(PartyService),
        provideAutoSpy(ImmsBCEformsResource),
      ],
    });
    resolver = TestBed.inject(immsBCEformsResolver);
  });

  it('should be created', () => {
    expect(resolver).toBeTruthy();
  });
});
