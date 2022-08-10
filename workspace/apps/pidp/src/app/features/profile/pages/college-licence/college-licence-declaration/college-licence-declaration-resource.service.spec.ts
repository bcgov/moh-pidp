import { TestBed } from '@angular/core/testing';

import { provideAutoSpy } from 'jest-auto-spies';

import { ApiHttpClient } from '@app/core/resources/api-http-client.service';
import { ToastService } from '@app/core/services/toast.service';

import { CollegeLicenceDeclarationResource } from './college-licence-declaration-resource.service';

describe('CollegeLicenceDeclarationResource', () => {
  let service: CollegeLicenceDeclarationResource;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        CollegeLicenceDeclarationResource,
        provideAutoSpy(ApiHttpClient),
        provideAutoSpy(ToastService),
      ],
    });

    service = TestBed.inject(CollegeLicenceDeclarationResource);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
