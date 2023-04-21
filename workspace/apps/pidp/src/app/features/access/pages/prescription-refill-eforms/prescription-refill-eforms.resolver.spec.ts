import { TestBed } from '@angular/core/testing';

import { PrescriptionRefillEformsResolver } from './prescription-refill-eforms.resolver';
import { provideAutoSpy } from 'jest-auto-spies';
import { PartyService } from '@app/core/party/party.service';
import { PrescriptionRefillEformsResource } from './prescription-refill-eforms-resource.service';

describe('PrescriptionRefillEformsResolver', () => {
  let resolver: PrescriptionRefillEformsResolver;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        provideAutoSpy(PartyService),
        provideAutoSpy(PrescriptionRefillEformsResource),
      ]
    });
    resolver = TestBed.inject(PrescriptionRefillEformsResolver);
  });

  it('should be created', () => {
    expect(resolver).toBeTruthy();
  });
});
