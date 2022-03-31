import { TestBed } from '@angular/core/testing';

import { provideAutoSpy } from 'jest-auto-spies';

import { UtilsService } from '@app/core/services/utils.service';

import { LookupResource } from './lookup-resource.service';
import { LookupService } from './lookup.service';

describe('ConfigService', () => {
  let service: LookupService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        LookupService,
        provideAutoSpy(LookupResource),
        provideAutoSpy(UtilsService),
      ],
    });
    service = TestBed.inject(LookupService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
